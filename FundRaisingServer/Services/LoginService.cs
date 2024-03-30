using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;

namespace FundRaisingServer.Services;

public class LoginService(JwtTokenService jwtTokenService, IUserRepository userRepo): ILoginRepository
{
    private readonly JwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserRepository _userService = userRepo;

    // to know about the method below refer to its Interface
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        try
        {
            // validating the user
            if (! (await this._userService.CheckUserAsync(request.Email, request.Password)) ) return null;

            var user = await this._userService.GetUserByEmailAsync(request.Email);

            if (user == null) return null;
            // generate JwtToken if we have the user
            var token = this._jwtTokenService.GenerateJwtToken(user);

            var response = new LoginResponseDto()
            {
                Email = request.Email,
                Token = token,
                Status = true
            };
            
            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}