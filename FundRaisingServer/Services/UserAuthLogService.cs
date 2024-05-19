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
    public async Task<bool> SaveUserAuthLogAsync(int userCnic, UserEventType eventTypeEnum)
    {
        try
        {
            // adding the user auth log in DB
            await this._context.UserAuthLogs.AddAsync(new UserAuthLog()
            {
                UserCnic = userCnic,
                EventTimestamp = DateTime.UtcNow,
                EventType = eventTypeEnum.ToString().ToUpper()
            });
            await this._context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> DeleteUserAuthLogAsync(int userCnic)
    {
        try
        {
            var logsToDelete = await this._context.UserAuthLogs
                .Where(l => l.UserCnic == userCnic)
                .ToListAsync();
        
            this._context.UserAuthLogs.RemoveRange(logsToDelete);
            await this._context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}