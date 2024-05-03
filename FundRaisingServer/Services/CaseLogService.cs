using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace FundRaisingServer.Services;

public class CaseLogService (FundRaisingDbContext context): ICaseLogRepository
{
    // DbContext
    private readonly FundRaisingDbContext _context = context;
    
    // method below just add the case Log
    private async Task<bool> AddNewCaseLogAsync(AddCaseLogRequestDto caseLogRequestDto)
    {
        var userLog = await this._context.CaseLogs.AddAsync(new CaseLog()
        {
            LogType = caseLogRequestDto.LogType,
            LogTimestamp = DateTime.UtcNow,
            CaseId = caseLogRequestDto.CaseId
        });
        await this._context.SaveChangesAsync();
        return true;
    }
    
    
    // method below will update the case Log and if it doesn't exist it will create a new one
    public async Task<bool> AddOrUpdateCaseLogAsync(AddCaseLogRequestDto caseLogRequestDto)
    {
        // we need to get the current case log
        var caseLog = await this._context.CaseLogs.Where(l => l.CaseId == caseLogRequestDto.CaseId && l.LogType == caseLogRequestDto.LogType).FirstOrDefaultAsync();
        if (caseLog == null)
        {
            await this.AddNewCaseLogAsync(caseLogRequestDto);
            return true;
        };
        caseLog.LogTimestamp = DateTime.UtcNow;
        return true;
    }
    
}