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
    DELETED_DATE
}
public class CaseLogDto
{
    // use Enum to get the LogType instead of hardcoding the logType
    public CaseLogTypeEnum LogType { get; set; }
    public DateTime LogTimestamp { get; set; }
    public int CaseId { get; set; }
}