using FundRaisingServer.Models.DTOs.Cause;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;


public class CauseBankService (FundRaisingDbContext context): ICauseBankService {
    private readonly FundRaisingDbContext _context = context;

    public async Task<CauseBankResponseDto> GetBankAmount()
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
}