using FundRaisingServer.Models.DTOs;

namespace FundRaisingServer.Repositories
{
    public interface ICasesRepository
    {
        /*

        * The method below return an enumerable
        * object of all the CasesResponseDto 
        * From the Db

        * It uses the FromSqlRaw method to get
        * all the cases from the DB and uses
        * Parametrised query for protection 
        * against the SQL INJECTION

        */
        Task<IEnumerable<CaseResponseDto>> GetAllCasesAsync();


        /*

        * The method below return a single case
        * of CaseResponesDto

        * It uses the FromSqlRaw method to get
        * all the cases from the DB and uses
        * Parametrised query for protection 
        * against the SQL INJECTION

        ? @param id: The id of the case to be fetched
        */
        Task<CaseResponseDto?> GetCaseByIdAsync(int id); // Add this method

        /*

        * The method below adds a new case to the DB

        * It uses the FromSqlRaw method to get
        * all the cases from the DB and uses
        * Parametrised query for protection 
        * against the SQL INJECTION

        ? @param caseDto: The case to be added

        */
        Task AddCaseAsync(AddCaseRequestDto caseDto);

        /*

        * The method below updates a case in the DB

        * It uses the FromSqlRaw method to get
        * all the cases from the DB and uses
        * Parametrised query for protection 
        * against the SQL INJECTION

        ? @param id: The id of the case to be updated
        ? @param caseDto: The case to be updated

        */
        Task UpdateCaseAsync(int id, CaseDto caseDto);

        /*
        
        * The method below deletes a case from the DB

        * It uses the FromSqlRaw method to get
        * all the cases from the DB and uses
        * Parametrised query for protection 
        * against the SQL INJECTION

        ? @param id: The id of the case to be deleted

        */
        Task DeleteCaseAsync(int id);

        Task<CaseResponseDto> VerifyCaseAsync(int id);
    }
}
