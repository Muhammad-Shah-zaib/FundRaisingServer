using FundRaisingServer.Models.DTOs.Cause;

public interface ICauseBankService
{
    Task<CauseBankResponseDto> GetBankAmount();
}