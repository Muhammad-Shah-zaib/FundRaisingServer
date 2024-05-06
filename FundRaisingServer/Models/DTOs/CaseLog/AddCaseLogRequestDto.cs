using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.CaseLog;

public class AddCaseLogRequestDto
{
    [Required]
    public string LogType { get; set; } = string.Empty;
    [Required]
    public int CaseId { get; set; }
}