using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using FundRaisingServer.Models.DTOs.CaseTransactions;

namespace FundRaisingServer.Services
{
    public class CaseTransactionService : ICaseTransactionRepository
    {
        private readonly FundRaisingDbContext _context;
        private readonly ICasesRepository _caseRepo;

        public CaseTransactionService(FundRaisingDbContext context, ICasesRepository casesRepo)
        {
            _context = context;
            _caseRepo = casesRepo;
        }

        // Retrieve all case transactions from the database
        public async Task<IEnumerable<CaseTransaction>> GetAllCaseTransactionsAsync()
        {
            try
            {
                return await _context.CaseTransactions.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Retrieve a specific case transaction by its ID
        public async Task<CaseTransaction?> GetCaseTransactionByIdAsync(int id)
        {
            try
            {
                var caseTransaction = await _context.CaseTransactions.FindAsync(id);
                if (caseTransaction == null) return null;
                
                return caseTransaction;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Create a new case transaction
        public async Task AddCaseTransactionAsync(AddCaseTransactionRequestDto caseTransaction)
        {
            try
            {
                await this._context.CaseTransactions.AddAsync(new CaseTransaction(){
                    CaseId = caseTransaction.CaseId,
                    DonorCnic = caseTransaction.DonorCnic,
                    TransactionAmount = caseTransaction.TransactionAmount,
                    TransactionLog = DateTime.UtcNow,
                });
                // need to fetch the case from the database and update the collected donations
                await this._caseRepo.UpdateCaseCollectedAmountAsync(caseId: caseTransaction.CaseId, amount: caseTransaction.TransactionAmount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Update an existing case transaction
        public async Task UpdateCaseTransactionAsync(CaseTransaction caseTransaction)
        {
            try
            {
                const string query = "UPDATE CaseTransactions " +
                                     "SET TransactionLog = @TransactionLog, " +
                                         "TransactionAmount = @TransactionAmount, " +
                                         "CaseId = @CaseId, " +
                                         "DonorCnic = @DonorCnic " +
                                     "WHERE CaseTransactionId = @CaseTransactionId";
                await _context.Database.ExecuteSqlRawAsync(query,
                    new SqlParameter("@TransactionLog", caseTransaction.TransactionLog),
                    new SqlParameter("@TransactionAmount", caseTransaction.TransactionAmount),
                    new SqlParameter("@CaseId", caseTransaction.CaseId),
                    new SqlParameter("@DonorCnic", caseTransaction.DonorCnic),
                    new SqlParameter("@CaseTransactionId", caseTransaction.CaseTransactionId));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Delete a case transaction by its ID
        public async Task DeleteCaseTransactionAsync(int id)
        {
            try
            {
                const string query = "DELETE FROM CaseTransactions WHERE CaseTransactionId = @CaseTransactionId";
                await _context.Database.ExecuteSqlRawAsync(query,
                    new SqlParameter("@CaseTransactionId", id));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
