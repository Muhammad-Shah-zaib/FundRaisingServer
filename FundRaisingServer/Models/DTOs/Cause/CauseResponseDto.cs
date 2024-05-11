namespace FundRaisingServer.Models.DTOs.Cause;

public class CauseResponseDto
{
    public int CauseId { get; set; }
    public string CauseTitle { get; set; } = string.Empty;
    public string CauseDescription { get; set; } = string.Empty;
    public decimal CollectedDonation { get; set; }
}