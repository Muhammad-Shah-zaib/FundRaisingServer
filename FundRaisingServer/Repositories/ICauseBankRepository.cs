using FundRaisingServer.Models.DTOs.Cause;

namespace FundRaisingServer.Repositories;

public interface ICauseBankService
{
    /*
     * The following method get all
     * current Donations from the
     * cause table and return that
     * as a CauseBankResponseDto
     */
    Task<CauseBankResponseDto> GetBankAmountAsync();
    
    /*
     * The following method get all
     * causes from the cause table
     * and return that as a list of
     * CauseResponseDto
     */
    Task<IEnumerable<CauseResponseDto>> GetAllCausesAsync();

}