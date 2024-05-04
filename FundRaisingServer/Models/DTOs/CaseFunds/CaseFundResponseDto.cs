namespace FundRaisingServer.Models.DTOs.CaseFunds;

public class CaseFundResponseDto : CaseFundsDto
{
    public int CaseFundId { get; set; }
    public int CaseId { get; set; }
}