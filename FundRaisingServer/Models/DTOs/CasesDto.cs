namespace FundRaisingServer.Models.DTOs;

public class CasesDto
{
    public int CaseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly CreatedDate { get; set; }
    public string? CauseName { get; set; }
    public int CauseId { get; set; }
}