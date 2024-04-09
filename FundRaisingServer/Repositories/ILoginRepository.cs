using FundRaisingServer.Models.DTOs.UserAuth;

namespace FundRaisingServer.Repositories;

public interface ILoginRepository
{
    /*
     * the method below will try logging
     * the user in, if and only if thef
     * credentials are correct to the
     * credentials present in the Db.
     *
     * This method return LoginResponseDto that contains
     * the user email and a JwtToken(string) hence it is
     * dependant on some other Services too
     *
     * We can use this method by logging the management
     * staff in since the JwtToken is our authentication scheme
     * and most of our endpoints will be protected by it
     */
    public Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}