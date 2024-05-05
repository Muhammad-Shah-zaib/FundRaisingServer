namespace FundRaisingServer.Models.DTOs.Case;

public class CaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CauseName { get; set; } = string.Empty;
    public decimal RequiredDonations {get;set;}
    public bool VerifiedStatus { get; set; }
}