using FundRaisingServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FundRaisingServer.Repositories
{
    public interface ICaseTransactionRepository
    {
        Task<IEnumerable<CaseTransaction>> GetAllCaseTransactionsAsync();
        Task<CaseTransaction> GetCaseTransactionByIdAsync(int id);
        Task CreateCaseTransactionAsync(CaseTransaction caseTransaction);
        Task UpdateCaseTransactionAsync(CaseTransaction caseTransaction);
        Task DeleteCaseTransactionAsync(int id);
    }
}
