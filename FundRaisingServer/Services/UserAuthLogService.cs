using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace FundRaisingServer.Services;

public class UserAuthLogService(FundRaisingDbContext context, IUserRepository userRepo) : IUserAuthLogRepository
{
    private readonly FundRaisingDbContext _context = context;
    private readonly IUserRepository _userRepo = userRepo;
    public async Task<bool> SaveUserAuthLogAsync(string email, UserEventType eventTypeEnum)
    {
        try
        {
            // getting the user to get his/her id
            var user = await this._userRepo.GetUserByEmailAsync(email);
            if (user == null) return false;

            /*
             * Now since we have the user we can get its id and
             * store its log
             */

            var userAuthLogDto = new UserAuthLogDto()
            {
                EventType = eventTypeEnum,
                EventTimestamp = DateTime.UtcNow,
                UserId = user.UserCnic
            };

            // we will check if the user has already a log of UserEventType or not
            // this can only happened in case of already existing Last_Login event
            // and also for Last_Update
            const string checkAuthLogQuery = "SELECT * FROM [dbo].[User_Auth_Log] WHERE [User_CNIC] = @UserCnic AND [Event_Type] = @EventType";
            var userAuthLog = await this._context.UserAuthLogs.FromSqlRaw(checkAuthLogQuery,
                new SqlParameter("@UserCnic", userAuthLogDto.UserId),
                new SqlParameter("@EventType", userAuthLogDto.EventType.GetDisplayName()))
                .SingleOrDefaultAsync();

            if (userAuthLog == null)
            {
                // while saving the event_type since it is an enum so we need to convert its value into string
                const string insertQuery = "INSERT INTO User_Auth_Log VALUES (@EventType, @EventTimestamp, @UserCnic) ";
                await this._context.Database.ExecuteSqlRawAsync(insertQuery,
                    new SqlParameter("@EventType", userAuthLogDto.EventType.GetDisplayName()),
                    new SqlParameter("@EventTimestamp", userAuthLogDto.EventTimestamp),
                    new SqlParameter("@UserCnic", userAuthLogDto.UserId));
                await this._context.SaveChangesAsync();
                return true;
            }
            // updating the log if the log already exist
            else
                await this.UpdateUserAuthLogAsync(user.UserCnic, eventTypeEnum);

            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

    }

    private async Task<bool> UpdateUserAuthLogAsync(int UserCnic, UserEventType eventTypeEnum)
    {
        const string updateQuery =
            "UPDATE User_Auth_log SET Event_Timestamp = DEFAULT  WHERE User_CNIC = @UserCnic AND Event_Type = @eventType";

        await this._context.Database.ExecuteSqlRawAsync(updateQuery,
            new SqlParameter("@UserCnic", UserCnic),
            new SqlParameter("@eventType", eventTypeEnum.GetDisplayName()));
        await this._context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAuthLogAsync(int UserCnic)
    {
        try
        {
            const string deleteQuery = "DELETE User_Auth_Log WHERE User_CNIC = @UserCnic";
            await this._context.Database.ExecuteSqlRawAsync(deleteQuery, new SqlParameter("@UserCnic", UserCnic));
            await this._context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

    }
}