using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;


public enum UserEventType
{
    [Display(Name = "REGISTRATION")]
    Registration,
    
    [Display(Name = "LOGIN")]
    Login,
    
    [Display(Name = "UPDATED")]
    Updated
}

public class UserAuthLogDto
{
    [Required]
    public UserEventType EventType { get; set; }

    [Required] public DateTime EventTimestamp { get; set; }

    [Required] public int UserId { get; set; }
}