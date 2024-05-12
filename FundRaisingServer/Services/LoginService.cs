using FundRaisingServer.Models.DTOs.UserAuth;
using FundRaisingServer.Repositories;

namespace FundRaisingServer.Services;

public class LoginService(IJwtTokenRepository jwtTokenService, IUserRepository userRepo, IUserAuthLogRepository userAuthLogRepo) : ILoginRepository
{
    private readonly IJwtTokenRepository _jwtTokenService = jwtTokenService;
    private readonly IUserRepository _userService = userRepo;
    private readonly IUserAuthLogRepository _userAuthLogRepo = userAuthLogRepo;

    // to know about the method below refer to its Interface
    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        try
        {
            // validating the user
            if (!await this._userService.CheckUserAsync(request.Email, request.Password)) return null;

            // we need to add the log of last login in the db
            try
            {
                await this._userAuthLogRepo.SaveUserAuthLogAsync(request.Email, UserEventType.Last_Login);
            }
            catch (Exception e)
            {
                // not terminating the program even if the log is not saved just logging the error out...
                Console.WriteLine(e);
                throw;
            }

            var user = await this._userService.GetUserByEmailAsync(request.Email);

            if (user == null) return null;
            // generate JWTToken if we have the user
            var token = this._jwtTokenService.GenerateJwtToken(user);

            var response = new LoginResponseDto()
            {
                UserCnic = user.UserCnic,
                Email = request.Email,
                Token = token,
                FirstName = user.FirstName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
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