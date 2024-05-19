using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;

public class UserTypeDto
{
    [Required] public int UserCnic { get; set; }
    [Required] public string UserType { get; set; } = string.Empty;
}