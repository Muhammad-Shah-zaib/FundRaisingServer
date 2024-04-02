using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;

public class UserTypeDto
{
    [Required] 
    public string Email { get; set; } = string.Empty;
    [Required] 
    public string UserType { get; set; } = string.Empty;
}