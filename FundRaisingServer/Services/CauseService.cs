using FundRaisingServer.Dtos;
using FundRaisingServer.Repositories;

namespace FundRaisingServer.Services;

public class CauseService (FundRaisingDbContext context): ICauseRepository
{
    FundRaisingDbContext _context = context;
    public async Task<bool> AddCauseAsync(  CauseDto causeDto )
    {

        try
            {
                // we need to create a new cause and assign the values from the causeDto
                var newCause = new Cause
                {
                    CauseTitle = causeDto.CauseTitle,
                    Description = causeDto.Description,
                    ClosedStatus = false,
                    CollectedAmount = 0 // Assuming initial collected amount is zero
                };
                // now we can add cause to db and save changes
                await _context.Causes.AddAsync(newCause);
                await _context.SaveChangesAsync();
                
                /*
                * since there are no triggers
                * in the db, so in order to 
                * make the logs we need to 
                * get the latest cause that we 
                * have stored and then create
                * a new log for it
                */
                var latestCause = _context.Causes.OrderByDescending(c => c.CauseId).FirstOrDefault();

                var newCauseLog = new CauseLog
                {
                    LogType = "CREATED",
                    LogTimestamp = DateTime.UtcNow,
                    CauseTitle = causeDto.CauseTitle,
                    Description = causeDto.Description,
                    UserCnic = causeDto.UserCnic,
                    CauseId = newCause.CauseId
                };

                _context.CauseLogs.Add(newCauseLog);
                await _context.SaveChangesAsync();

                // returning true if everything goes well
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);  
                throw;
            }
    }
}