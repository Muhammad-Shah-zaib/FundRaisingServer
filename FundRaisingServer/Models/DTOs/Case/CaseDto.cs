namespace FundRaisingServer.Models.DTOs;

public class CaseDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CauseName { get; set; } = string.Empty;
    public bool VerifiedStatus { get; set; }
}