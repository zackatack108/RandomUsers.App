using RandomUsers.App.Models;

namespace RandomUsers.App.Services
{
    public interface IRandomUsersApiService
    {
        Task<List<UserModel>> GetRandomUsers(string gender, int total, bool refreshRequired = false);
    }
}
