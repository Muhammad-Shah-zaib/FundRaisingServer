using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using FundRaisingServer.Models.DTOs.CaseLog;
using FundRaisingServer.Models.DTOs.Case;

namespace FundRaisingServer.Services
{
    public class CasesService(FundRaisingDbContext context) : ICasesRepository
    {
        private readonly FundRaisingDbContext _context = context;

        public async Task<IEnumerable<CaseResponseDto>> GetAllVerifiedCasesAsync(){
            try
            {
                var cases = await this._context.Cases
                    .Where(c => !c.ResolveStatus)
                    .Where(c => c.VerifiedStatus)
                    .Include(c => c.CaseLogs)
                    .Select(c => new CaseResponseDto()
                    {
                        CaseId = c.CaseId,
                        Title = c.Title,
                        Description = c.Description,
                        VerifiedStatus = c.VerifiedStatus,
                        CauseName = c.CauseName ?? string.Empty,
                        CollectedDonations = c.CollectedAmount,
                        RequiredDonations = c.RequiredAmount,
                        RemainingDonations = c.RemainingAmount ?? 0,
                        CaseLogs = c.CaseLogs
                            .Where(l => l.CaseId == c.CaseId)
                            .Where(l => l.LogType == "Created")
                            .Select(l => new CaseLogDto()
                            {
                                LogType = l.LogType,
                                LogDate = l.LogTimestamp.Date.ToString("yyyy-MM-dd"),
                                LogTime = l.LogTimestamp.TimeOfDay,
                            })
                            .ToList()
                    })
                    .OrderBy(c => c.CaseId)
                    .OrderByDescending(c => c.VerifiedStatus)
                    .ToListAsync();
                return cases;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // the method to get all the cases from the DB
        public async Task<IEnumerable<CaseResponseDto>> GetAllCasesAsync()
        {
            try
            {
                var cases = await this._context.Cases
                    .Where(c => !c.ResolveStatus)
                    .Select(c => new CaseResponseDto()
                    {
                        CaseId = c.CaseId,
                        Title = c.Title,
                        Description = c.Description,
                        VerifiedStatus = c.VerifiedStatus,
                        CauseName = c.CauseName ?? string.Empty,
                        CollectedDonations = c.CollectedAmount,
                        RequiredDonations = c.RequiredAmount,
                        RemainingDonations = c.RemainingAmount ?? 0,
                        CaseLogs = c.CaseLogs
                            .Where(l => l.CaseId == c.CaseId)
                            .Select(l => new CaseLogDto()
                            {
                                LogType = l.LogType,
                                LogDate = l.LogTimestamp.Date.ToString("yyyy-MM-dd"),
                                LogTime = l.LogTimestamp.TimeOfDay,
                            })
                            .ToList()
                    })
                    .OrderBy(c => c.CaseId)
                    .OrderByDescending(c => c.VerifiedStatus)
                    .ToListAsync();
                return cases;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // the method to get a single case from the DB
        public async Task<CaseResponseDto?> GetCaseByIdAsync(int id) // Add this method
        {
            try
            {
                // using the query method ot get the cases from the DB
                var singleCase = await _context.Cases.FindAsync(id);

                if (singleCase == null) return null; // Case not found

                // returning the founded case
                return new CaseResponseDto()
                {
                    CaseId = singleCase.CaseId,
                    Title = singleCase.Title,
                    Description = singleCase.Description,
                    VerifiedStatus = singleCase.VerifiedStatus,
                    CollectedDonations = singleCase.CollectedAmount,
                    RequiredDonations = singleCase.RequiredAmount,
                    RemainingDonations = singleCase.RemainingAmount,
                    CauseName = singleCase.CauseName ?? string.Empty,
                };


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // the method to add a new case to the DB
        public async Task AddCaseAsync(AddCaseRequestDto caseRequestDto)
        {
            try
            {
                // adding the case to the DB
                await this._context.Cases.AddAsync(new Case()
                {
                    Title = caseRequestDto.Title,
                    Description = caseRequestDto.Description,
                    CauseName = caseRequestDto.CauseName,
                    VerifiedStatus = caseRequestDto.VerifiedStatus,
                    RequiredAmount = caseRequestDto.RequiredDonations,
                    ResolveStatus = false,
                    CollectedAmount = 0 // initially there will be no amount collected
                });
                await _context.SaveChangesAsync();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // the method to delete a case from the DB
        public async Task DeleteCaseAsync(int id)
        {
            try
            {
                // at first, we need to delete the logs
                const string deleteQuery = "DELETE Case_Log WHERE Case_ID = @CaseId";
                await this._context.Database.ExecuteSqlRawAsync(deleteQuery,
                    new SqlParameter("@CaseId", id));

                // now we can delete the case
                const string query = "DELETE Cases WHERE Case_ID = @CaseId";

                /* 
                * executing the query using the Parameterized Query 
                * for protection against Sql Injection
                */
                await _context.Database.ExecuteSqlRawAsync(query,
                    new SqlParameter("@CaseId", id));
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // the method to update a case in the DB
        public async Task UpdateCaseAsync(int id, UpdateCaseRequestDto caseDto)
        {
            try
            {
                // Checking for the Case
                var existingCase = await _context.Cases.FindAsync(id) ?? throw new ArgumentException("Case not found");

                // now we need to add the case
                existingCase.Title = caseDto.Title;
                existingCase.Description = caseDto.Description;
                existingCase.CauseName = caseDto.CauseName;
                existingCase.VerifiedStatus = caseDto.VerifiedStatus;
                existingCase.RequiredAmount = caseDto.RequiredDonations;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // the method to verify a case in the DB
        public async Task<CaseResponseDto> VerifyCaseAsync(int id)
        {
            // get the case from db
            var existingCase = await _context.Cases.FindAsync(id) ?? throw new ArgumentException("Case not found");

            // now we will verify the case
            const string query = "UPDATE [Cases] SET [Verified_Status] = 1 WHERE Case_ID = @CaseId";

            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@CaseId", id));
            await this._context.SaveChangesAsync();

            // returning the updated case
            return new CaseResponseDto()
            {
                CaseId = id,
                Title = existingCase.Title,
                Description = existingCase.Description,
                CauseName = existingCase.CauseName,
                VerifiedStatus = true
            };

        }

        // the method to un-verify a case in the DB
        public async Task<CaseResponseDto> UnVerifyCaseAsync(int id)
        {
            // WE NEED TO HANDLE teh edge CASES... Will DO THIS LATER

            // get the case from db
            var existingCase = await _context.Cases.FindAsync(id) ?? throw new ArgumentException("Case not found");

            // now we will verify the case
            const string query = "UPDATE [Cases] SET [Verified_Status] = 0 WHERE Case_ID = @CaseId";

            await this._context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@CaseId", id));
            await this._context.SaveChangesAsync();

            // returning the updated case
            return new CaseResponseDto()
            {
                CaseId = id,
                Title = existingCase.Title,
                Description = existingCase.Description,
                CauseName = existingCase.CauseName,
                VerifiedStatus = false
            };
        }

        // the method to update the collectedAmount of a case in the DB - Call this after transation is made to update the case Colelcted Amount
        public async Task<CaseResponseDto?> UpdateCaseCollectedAmountAsync(int caseId, decimal amount)
        {
            try
            {
                var existingCase = await this._context.Cases.FindAsync(caseId);
                if (existingCase == null) return null;
                existingCase.CollectedAmount += amount;
                await this._context.SaveChangesAsync();
                return new CaseResponseDto()
                {
                    CaseId = existingCase.CaseId,
                    CollectedDonations = existingCase.CollectedAmount,
                    RemainingDonations = existingCase.RemainingAmount ?? 0,
                    RequiredDonations = existingCase.RequiredAmount
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // this method is used to resolve a case
        public async Task<bool?> ResolveCaseAsync(int id, int userCnic)
        {
            try
            {
                var existingCase = await this._context.Cases.FindAsync(id);
                if (existingCase == null) return null;
                existingCase.ResolveStatus = true;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}