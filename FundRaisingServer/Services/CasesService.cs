using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;

public class CasesService (FundRaisingDbContext context): ICasesRepository
{
    private readonly FundRaisingDbContext _context = context;

    public async Task<List<CasesDto>> GetAllCasesAsync()
    {
        try
        {
            const string query = "SELECT * FROM Cases";
            var cases = await this._context.Cases.FromSqlRaw(query)
                .Select(c => new CasesDto()
                {
                    CaseId = c.CaseId,
                    Title = c.Title!,
                    Description = c.Description!,
                    CreatedDate = c.CreatedDate,
                    CauseName = c.Cause.Title,
                    CauseId = c.CauseId
                })
                .ToListAsync();
            
            return cases;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}