using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface IUserRepository
    {
        Task<User> AuthenticateAsync(string username, string password);
    }
}
