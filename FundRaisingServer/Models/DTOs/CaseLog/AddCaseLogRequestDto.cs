using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.CaseLog;

public class AddCaseLogRequestDto
{
    [Required]
    public string LogType { get; set; }
    [Required]
    public int CaseId { get; set; }
}