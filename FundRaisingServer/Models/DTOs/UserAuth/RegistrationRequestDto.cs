using System.ComponentModel.DataAnnotations;

namespace FundRaisingServer.Models.DTOs.UserAuth;

public class RegistrationRequestDto
{
    /*
     * User_Id is already set to AUTO_INCREMENT
     * So no need to take the UserId from the user
     * as it will automatically be handles by the
     * MS SQL SERVER
     */
    
    public int? UserId { get; set; }
    [Required]
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [Required] 
    public string UserType { get; set; } = string.Empty;
    
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}