using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
