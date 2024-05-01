using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using FundRaisingServer.Models.DTOs.CaseLog;

namespace FundRaisingServer.Services
{
    public class CasesService : ICasesRepository
    {
        private readonly FundRaisingDbContext _context;

        public CasesService(FundRaisingDbContext context)
        {
            _context = context;
        }

        // the method to get all the cases from the DB
        public async Task<IEnumerable<CaseResponseDto>> GetAllCasesAsync()
        {
            try
            {
                var cases = await this._context.Cases
                    .Select(c => new CaseResponseDto()
                    {
                        CaseId = c.CaseId,
                        Title = c.Title,
                        Description = c.Description,
                        VerifiedStatus = c.VerifiedStatus,
                        CauseName = c.CauseName ?? string.Empty,
                        CaseLogs = c.CaseLogs
                            .Where(l => l.CaseId == c.CaseId)
                            .Select(l => new CaseLogDto()
                            {
                                LogType = l.LogType,
                                LogTimestamp = l.LogTimestamp
                            })
                            .ToList()
                    })
                    .ToListAsync();

                return cases;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // the methid to get a single case from the DB
        public async Task<CaseResponseDto?> GetCaseByIdAsync([FromBody] int id) // Add this method
        {
            try
            {
                // using the query method ot get the cases from the DB
                const string query = "SELECT * FROM Cases WHERE Case_ID = @CaseId";
                var singleCase = await _context.Cases.FromSqlRaw(query,
                        new SqlParameter("@CaseId", id))
                    .SingleOrDefaultAsync(c => c.CaseId == id);

                if (singleCase == null) return null; // Case not found

                // returning the founded case
                return new CaseResponseDto()
                {
                    CaseId = singleCase.CaseId,
                    Title = singleCase.Title,
                    Description = singleCase.Description,
                    VerifiedStatus = singleCase.VerifiedStatus,
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
                // Inserting the new case into the db via query method
                const string query = "INSERT INTO Cases VALUES (@Title, @Description, @CauseName, @VerifiedStatus)";

                // providing the params for protecting against Sql Injection
                await _context.Database.ExecuteSqlRawAsync(query,
                    new SqlParameter("@Title", caseRequestDto.Title),
                    new SqlParameter("@Description", caseRequestDto.Description),
                    new SqlParameter("@CauseName", caseRequestDto.CauseName),
                    new SqlParameter("@VerifiedStatus", caseRequestDto.VerifiedStatus));
                // saving the changes
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
                // query for deleting the case
                const string query = "DELETE FROM Cases WHERE Case_ID = @CaseId";

                // Checking for the Case
                var existingCase = await this.GetCaseByIdAsync(id) ?? throw new ArgumentException("Case not found");

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
        public async Task UpdateCaseAsync(int id, CaseDto caseDto)
        {
            try
            {
                // query for updating the case
                const string query = "UPDATE [Cases] SET [Title] = @Title, [Description] = @Description, [Cause_Name] = @CauseName, [Verified_Status] = @VerifiedStatus WHERE Case_ID = @CaseId";

                // Checking for the Case
                var existingCase = await _context.Cases.FindAsync(id) ?? throw new ArgumentException("Case not found");

                // executing the query using the Parameterized Query 
                // for protection against Sql Injection
                await _context.Database.ExecuteSqlRawAsync(query,
                    new SqlParameter("@Title", caseDto.Title),
                        new SqlParameter("@Description", caseDto.Description),
                        new SqlParameter("@CauseName", caseDto.CauseName),
                        new SqlParameter("@VerifiedStatus", caseDto.VerifiedStatus),
                        new SqlParameter("@CaseId", id));
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
            // WE NEED TO HANDLE teh edge CASES... WILL DO THIS LATER

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

        // the method to unverify a case in the DB
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
    }
}
