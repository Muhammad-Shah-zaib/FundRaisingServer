using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.CaseLog;


public enum CaseLogTypeEnum
{
    [Display(Name = "CREATED_DATE")]
    CREATED_DATE,

    [Display(Name = "UPDATED_DATE")]
    UPDATED_DATE,

    [Display(Name = "RESOLVED_DATE")]
    RESOLVED_DATE,

    [Display(Name = "DELETED_DATE")]
    DELETED_DATE,
    [Display(Name = "CLOSED_DATE")]
    CLOSED_DATE,
    [Display(Name = "VERIFIED_DATE")]
    VERIFIED_DATE,
    [Display(Name = "UNVERIFIED_DATE")]
    UNVERIFIED_DATE

}

public class CaseLogDto
{
    // use Enum to get the LogType instead of hardcoding the logType
    public string LogType { get; set; } = string.Empty;
    public string LogDate { get; set; } = string.Empty;
    public TimeSpan LogTime { get; set; }
    public decimal CollectedAmount { get; set; }
    public int UserCnic { get; set; } 
   public int CaseId { get; set; }
}