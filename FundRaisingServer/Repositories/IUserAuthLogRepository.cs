using FundRaisingServer.Models.DTOs.UserAuth;

namespace FundRaisingServer.Repositories;

public interface IUserAuthLogRepository
{
    public Task<bool> SaveUserAuthLogAsync(string email, UserEventType eventTypeEnum);
}