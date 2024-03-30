using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;

namespace FundRaisingServer.Services;

public class LoginService(FundRaisingDbContext context, JwtTokenService jwtTokenService, IUserRepository userRepo)
{
    private readonly FundRaisingDbContext _context = context;
    private readonly JwtTokenService _jwtTokenService = jwtTokenService;
    private readonly IUserRepository _userService = userRepo;

    public async Task<LoginResponseDto?> LoginAsync(string email, string password)
    {
        
        
        try
        {
            // validating the user
            if (! (await this._userService.CheckUserAsync(email, password)) ) return null;

            var user = await this._userService.GetUserByEmailAsync(email);

            if (user == null) return null;
            // generate JwtToken if we have the user
            var token = this._jwtTokenService.GenerateJwtToken(user);

            var response = new LoginResponseDto()
            {
                Email = email,
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