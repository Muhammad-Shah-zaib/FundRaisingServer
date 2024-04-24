namespace FundRaisingServer.Models.DTOs.UserAuth;

public class UserResponseDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime RegistrationTimeStamp { get; set; }
    public DateTime LastLoginTimeStamp { get; set; }
}