using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FundRaisingServer.Services;


public class DonatorService (FundRaisingDbContext context): IDonatorRepository
{
    private readonly FundRaisingDbContext _context = context;
    public async Task<Int32> GetDonatorCountAsync()
    {
        try
        {
            return await 
                this._context.Donators
                .CountAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}