using FundRaisingServer.Models.DTOs.Case;
using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class CaseLogService (FundRaisingDbContext context): ICaseLogRepository
{
    // DbContext
    private readonly FundRaisingDbContext _context = context;
    
    // method below just add the case Log
    public async Task<bool> AddNewCaseLogAsync(AddCaseLogRequestDto caseLogRequestDto ,AddCaseToLogRequestDto addCaseToLogRequestDto)
    {
        var userLog = await this._context.CaseLogs.AddAsync(new CaseLog()
        {
            LogType = caseLogRequestDto.LogType,
            LogTimestamp = DateTime.UtcNow,
            CaseId = caseLogRequestDto.CaseId,
            Title = addCaseToLogRequestDto.Title,
            CauseName = addCaseToLogRequestDto.CauseName,
            RequiredAmount = addCaseToLogRequestDto.RequiredDonations,
            CollectedAmount = addCaseToLogRequestDto.CollectedDonations,
            RemainingAmount = addCaseToLogRequestDto.RemainingDonations,
            VerifiedStatus = addCaseToLogRequestDto.VerifiedStatus,
            ResolvedStatus = addCaseToLogRequestDto.ResolvedStatus,
            Description = addCaseToLogRequestDto.Description
        });
        await this._context.SaveChangesAsync();
        return true;
    }
}