using FundRaisingServer.Models.DTOs.Cause;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;


public class CauseBankService (FundRaisingDbContext context): ICauseBankService {
    private readonly FundRaisingDbContext _context = context;

    public async Task<CauseBankResponseDto> GetBankAmountAsync()
    {
        try
        {
            var totalCurrentDonations = await this._context.Causes.SumAsync(c => c.CollectedAmount);
            return new CauseBankResponseDto()
            {
                TotalCurrentDonations = totalCurrentDonations
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<IEnumerable<CauseResponseDto>> GetAllCausesAsync()
    {
        return await this._context.Causes
            .Select(c =>new CauseResponseDto()
            {
                CauseTitle = c.CauseTitle,
                CauseDescription = c.Description ?? string.Empty,
                CollectedDonation = c.CollectedAmount
            })
            .ToListAsync();
        
    }
}