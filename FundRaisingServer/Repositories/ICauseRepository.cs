using FundRaisingServer.Dtos;

namespace FundRaisingServer.Repositories
{
    public interface ICauseRepository
    {
        Task<bool> AddCauseAsync(CauseDto causeDto);
    }
}