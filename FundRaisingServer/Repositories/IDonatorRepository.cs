namespace FundRaisingServer.Repositories;

public interface IDonatorRepository
{
    Task<Int32> GetDonatorCountAsync();
}