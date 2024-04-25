using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;


public enum UserEventType
{
    [Display(Name = "Registration")]
    Registration,
    
    [Display(Name = "Last_Login")]
    Last_Login
}

public class UserAuthLogDto
{
    [Required]
    public UserEventType EventType { get; set; }

    [Required] public DateTime EventTimestamp { get; set; }

    [Required] public int UserId { get; set; }
}