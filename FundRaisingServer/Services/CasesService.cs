using FundRaisingServer.Models.DTOs;
using FundRaisingServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FundRaisingServer.Services
{
    public class CasesService : ICasesRepository
    {
        private readonly FundRaisingDbContext _context;

        public CasesService(FundRaisingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CaseResponseDto>> GetAllCasesAsync()
        {
            try
            {
                // using query to fetch the DATA FROM THE DB
                const string query = "SELECT * FROM Cases";

                var cases = await this._context.Cases.FromSqlRaw(query)
                    .Select(c => new CaseResponseDto()
                    {
                        CaseId = c.CaseId,
                        Title = c.Title!,
                        Description = c.Description!,
                        CreatedDate = DateTime.UtcNow,
                        CauseName = c.CauseName ?? string.Empty,
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
                    Title = singleCase.Title!,
                    Description = singleCase.Description!,
                    CreatedDate = DateTime.UtcNow,
                    CauseName = singleCase.CauseName ?? string.Empty,
                };


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task AddCaseAsync(AddCaseRequestDto caseRequestDto)
        {
            try
            {
                // Inserting the new case into the db via query method
                const string query = "INSERT INTO Cases VALUES (@Title, @Description, @Created_Date, @CauseName)";

                // providing the params for protecting against Sql Injection
                await _context.Database.ExecuteSqlRawAsync(query,
                    new SqlParameter("@Title", caseRequestDto.Title),
                    new SqlParameter("@Description", caseRequestDto.Description),
                    new SqlParameter("@Created_Date", DateTime.UtcNow),
                    new SqlParameter("@CauseName", caseRequestDto.CauseName));
                // saving the changes
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
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
        public async Task UpdateCaseAsync(int id, CaseDto caseDto)
        {
            try
            {
                // query for updating the case
                const string query = "UPDATE [Cases] SET [Title] = @Title, [Description] = @Description, [Cause_Name] = @CauseName WHERE Case_ID = @CaseId";

                // Checking for the Case
                var existingCase = await _context.Cases.FindAsync(id) ?? throw new ArgumentException("Case not found");

                // executing the query using the Parameterized Query 
                // for protection against Sql Injection
                await _context.Database.ExecuteSqlRawAsync(query,
                    new SqlParameter("@Title", caseDto.Title),
                        new SqlParameter("@Description", caseDto.Description),
                        new SqlParameter("@CauseName", caseDto.CauseName),
                        new SqlParameter("@CaseId", id));
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
