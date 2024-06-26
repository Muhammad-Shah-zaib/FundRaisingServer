using FundRaisingServer.Models.DTOs.Cause;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;


public class CauseBankService (FundRaisingDbContext context): ICauseBankService {
    private readonly FundRaisingDbContext _context = context;

    public async Task<IEnumerable<CauseResponseDto>> GetAllCausesAsync()
    {
        try
        {
            return await this._context.Causes
            .Where(c => !c.ClosedStatus)
            .Select(c => new CauseResponseDto()
            {
                CauseId = c.CauseId,
                CauseDescription = c.Description ?? string.Empty,
                CauseTitle = c.CauseTitle,
                CollectedDonation = c.CollectedAmount
            })
            .ToListAsync();
        }catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CauseBankResponseDto> GetBankAmountAsync()
    {
        try
        {
            var totalCurrentDonations = await this._context.Causes
            .Where(c => !c.ClosedStatus)
            .SumAsync(c => c.CollectedAmount);
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

    public async Task<DonationSoFarResponse> GetDonationsSoFarAsync()
    {
        try
        {
            var totalDonations =  await this._context.Causes
                .SumAsync(c => c.CollectedAmount);

                return new DonationSoFarResponse(){
                    TotalDonations = totalDonations
                };
        }catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

    }

    public async Task<CauseResponseDto?> GetCauseByIdAsync(int id)
    {
        try
        {
            var cause = await this._context.Causes.FindAsync(id);
            if (cause == null) return null;
            return new CauseResponseDto()
            {
                CauseId = cause.CauseId,
                CauseDescription = cause.Description ?? string.Empty,
                CauseTitle = cause.CauseTitle,
                CollectedDonation = cause.CollectedAmount
            };
        }catch(Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}