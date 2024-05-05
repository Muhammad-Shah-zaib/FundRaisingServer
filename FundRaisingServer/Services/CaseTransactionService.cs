using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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
        public async Task<CaseTransaction> GetCaseTransactionByIdAsync(int id)
        {
            try
            {
                return await _context.CaseTransactions.FindAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Create a new case transaction
        public async Task CreateCaseTransactionAsync(CaseTransaction caseTransaction)
        {
            try
            {
                await this._context.CaseTransactions.AddAsync(caseTransaction);
                var existingCase = await this._caseRepo.GetCaseByIdAsync(caseTransaction.CaseId ?? 0);

                existingCase!.CollectedDonations += caseTransaction.TransactionAmount;
                await this._context.SaveChangesAsync();

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
