using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Models.DTOs.CaseTransactions;

namespace FundRaisingServer.Repositories
{
    public interface ICaseTransactionRepository
    {
        Task<decimal> GetTotalDonationsAsync();
        Task<IEnumerable<CaseTransactionResponseDto>> GetAllCaseTransactionsAsync();
        Task<CaseTransaction?> GetCaseTransactionByIdAsync(int id);
        Task AddCaseTransactionAsync(AddCaseTransactionRequestDto caseTransaction);
        Task UpdateCaseTransactionAsync(CaseTransaction caseTransaction);
        Task DeleteCaseTransactionAsync(int id);

        Task AddCaseLogAsync(CaseLogDto caseLog);
    }
}
