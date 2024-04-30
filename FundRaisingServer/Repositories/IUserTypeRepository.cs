using FundRaisingServer.Models.DTOs.UserAuth;

namespace FundRaisingServer.Repositories;

public interface IUserTypeRepository
{
    /*
     * The method below will save the user type
     * by taking the email and userType (UserTypeDto)
     */
    public Task<bool> SaveUserTypeAsync(UserTypeDto userTypeDto);

    /*
    * The method below will 
    * delete the user type from the DB
    * by using the UserCnic
    
    * There is no need to verify the user Exist or not
    * as the UserCnic is already a foreign key in the User_Type
    */

    Task<bool> DeleteUserTypeByUserCnicAsync(int UserCnic);

    Task<bool> UpdateUserTypeByUserCnicAsync(int UserCnic, string userType);
}