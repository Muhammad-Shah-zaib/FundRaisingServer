namespace FundRaisingServer.Models.DTOs.UserAuth;
public class UserAuthLogsResponseDto
{
    public string EventType { get; set; } = string.Empty;
    public DateTime? EventTimestamp { get; set; }
}

public class UserResponseDto
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public IEnumerable<UserAuthLogsResponseDto>? UserAuthLogsList { get; set; }
    public string PhoneNo { get; set; } = string.Empty;
    public int Cms { get; set; }
}

