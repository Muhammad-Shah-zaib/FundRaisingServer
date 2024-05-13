using FundRaisingServer.Models.DTOs.CaseLog;

namespace FundRaisingServer.Repositories;

public interface ICaseLogRepository
{
    /*
     * The method below first need to
     * check if the provided logType
     * is already in the database or
     * not,
     * if not we need to add the new
     * log
     * if yes, we need ot update that
     * log
     */
    Task<bool> AddNewCaseLogAsync(AddCaseLogRequestDto caseLogRequestDto, AddCaseToLogRequestDto addCaseToLogRequestDto);
}