using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;


public enum UserEventType
{
    [Display(Name = "REGISTRATION")]
    Registration,
    
    [Display(Name = "LOGIN")]
    Last_Login,
    
    [Display(Name = "UPDATED")]
    Last_Update
}

public class UserAuthLogDto
{
    [Required]
    public UserEventType EventType { get; set; }

    [Required] public DateTime EventTimestamp { get; set; }

    [Required] public int UserId { get; set; }
}