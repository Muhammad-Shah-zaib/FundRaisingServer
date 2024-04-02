using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;

public class RegistrationResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    /*
     * Errors field will be utilized for some custom
     * errors but not for simple errors
     */
    public List<string>? Errors { get; set; }
}