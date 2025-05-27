using GameZone.Models;

namespace GameZone.Services
{
    public interface IRatingServies
    {
        Task AddRating(int gameId, string userId, int value);
        Task RemoveRating(int gameId, string userId);
        Rating GetUserRating(int gameId, string UserId);
        Task<double> GetAverageRating(int gameId);
        Task<int> GetRatingCount(int gameId);
    }
}
