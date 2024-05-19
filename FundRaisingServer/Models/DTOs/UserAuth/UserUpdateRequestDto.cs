using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;

public class UserUpdateRequestDto
{
    [Required] public int UserId { get; set; } = 0;
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public int Cms { get; set; }
    [Required] public string PhoneNo { get; set; } = string.Empty;
    public string? UserType { get; set; } = null;
}