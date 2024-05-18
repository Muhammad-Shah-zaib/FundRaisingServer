using FundRaisingServer.Models.DTOs.Case;
using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class CaseLogService(FundRaisingDbContext context) : ICaseLogRepository
{
    // DbContext
    private readonly FundRaisingDbContext _context = context;

    /*
     * The method below just add the case log
     * and does not verify if the case given exist
     * or not, in-fact, it takes Case as a @param
     */
    public async Task<bool> AddCaseLogAsync(Case existingCase, CaseLogTypeEnum logType, int userCnic)
    {
        try
        {
            // sacing the case
            await this._context.CaseLogs.AddAsync(new CaseLog()
            {
                // log details
                LogType = logType.ToString(),
                LogTimestamp = DateTime.UtcNow,
                // case details
                CaseId = existingCase.CaseId,
                Title = existingCase.Title,
                Description = existingCase.Description,
                CollectedAmount = existingCase.CollectedAmount,
                RequiredAmount = existingCase.RequiredAmount,
                ResolvedStatus = existingCase.ResolveStatus,
                VerifiedStatus = existingCase.VerifiedStatus,
                CauseName = existingCase.CauseName,
                // userCNIC who caused the log
                UserCnic = userCnic
            });

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