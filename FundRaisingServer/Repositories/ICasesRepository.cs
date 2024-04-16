using FundRaisingServer.Models.DTOs;

namespace FundRaisingServer.Repositories;

public interface ICasesRepository

{
    public Task<List<CasesDto>> GetAllCasesAsync();
}