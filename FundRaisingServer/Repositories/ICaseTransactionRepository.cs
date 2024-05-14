using FundRaisingServer.Models;
using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Models.DTOs.CaseTransactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundRaisingServer.Repositories
{
    public interface ICaseTransactionRepository
    {
        Task<IEnumerable<CaseTransactionResponseDto>> GetAllCaseTransactionsAsync();
        Task<CaseTransaction?> GetCaseTransactionByIdAsync(int id);
        Task AddCaseTransactionAsync(AddCaseTransactionRequestDto caseTransaction);
        Task UpdateCaseTransactionAsync(CaseTransaction caseTransaction);
        Task DeleteCaseTransactionAsync(int id);

        Task AddCaseLogAsync(CaseLogDto caseLog);
    }
}
