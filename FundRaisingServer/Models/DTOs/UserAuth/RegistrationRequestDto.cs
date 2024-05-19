using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;

public class RegistrationRequestDto
{
    
    [Required] public string FirstName { get; set; } = string.Empty;
    // LastName can be null so it is not required
    public string LastName { get; set; } = string.Empty;
    [Required] public string UserType { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string PhoneNo { get; set; } = string.Empty;
    [Required] public int Cms { get; set; }
    [Required] public string Password { get; set; } = string.Empty;
}