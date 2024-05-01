using FundRaisingServer.Models.DTOs.CaseLog;
using Microsoft.OpenApi.Extensions;

namespace FundRaisingServer.Services;

public class CaseLogService (FundRaisingDbContext context)
{
    // DbContext
    private readonly FundRaisingDbContext _context = context;

    public async Task<bool> AddNewCaseLogAsync(NewCaseLogRequestDto caseLogRequestDto)
    {
        var userLog = await this._context.CaseLogs.AddAsync(new CaseLog()
        {
            LogType = caseLogRequestDto.LogType,
            LogTimestamp = caseLogRequestDto.LogTimestamp,
            CaseId = caseLogRequestDto.CaseId
        });
        await this._context.SaveChangesAsync();
        return true;
    }
}