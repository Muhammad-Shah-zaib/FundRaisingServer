namespace FundRaisingServer.Models.DTOs.UserAuth;

public class LoginResponseDto
{
    public string Email { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public bool Status { get; set; }
    public List<string>? Errors { get; set; }
}