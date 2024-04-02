namespace FundRaisingServer.Repositories;

public interface IJwtTokenRepository
{
    /*
     * method below will generate a unique
     * jwt token using the user email
     * and the expiration is for about *10 days*
     */
    public string GenerateJwtToken(User user);
}