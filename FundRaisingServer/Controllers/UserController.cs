using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FundRaisingServer.Controllers;

[ApiController]
[Route("/[controller]")]
public class UserController (IUserRepository userRepo): ControllerBase
{
    private readonly IUserRepository _userRepo = userRepo;
    
    [HttpGet]
    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        return await this._userRepo.GetAllUsersAsync();
    }
}