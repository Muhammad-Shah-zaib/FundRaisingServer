using FundRaisingServer.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundRaisingServer.Repositories
{
    public interface ICasesRepository
    {
        Task<List<CasesDto>> GetAllCasesAsync();
        Task<CasesDto?> GetCaseByIdAsync(int id); // Add this method
        Task AddCaseAsync(CasesDto caseDto);
        Task UpdateCaseAsync(int id, CasesDto caseDto);
    }
}
