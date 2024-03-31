using FundRaisingServer.Models.DTOs.UserAuth;

namespace FundRaisingServer.Repositories;

public interface IUserTypeRepository
{
    /*
     * The method below will save the user type
     * by taking the email and userType (UserTypeDto)
     */
    public Task<bool> SaveUserTypeAsync(UserTypeDto userTypeDto);
}