using FundRaisingServer.Models.DTOs.UserAuth;

namespace FundRaisingServer.Repositories;

public interface IUserAuthLogRepository
{
    /*
     * This method save the logs
     * if specific log is already in there
     * then it update the logs instead of
     * inserting new log
     */
    public Task<bool> SaveUserAuthLogAsync(string email, UserEventType eventTypeEnum);
}