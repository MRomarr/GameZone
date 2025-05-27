using GameZone.Data;
using GameZone.Models;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class RatingServies : IRatingServies
    {
        private readonly ApplicationDbContext _context;
        public RatingServies(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddRating(int gameId, string userId, int value)
        {
            var rating = new Rating
            {
                GameId = gameId,
                UserId = userId,
                Value = value
            };
            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveRating(int gameId, string userId)
        {
            var rating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.GameId == gameId && r.UserId == userId);
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();
        }
        public  Rating GetUserRating(int gameId, string UserId) =>  _context.Ratings
            .FirstOrDefault(r => r.GameId == gameId && r.UserId == UserId); 
      public async Task<double> GetAverageRating(int gameId)
        {
            return await _context.Ratings
                .Where(r => r.GameId == gameId)
                .Select(r => (double?)r.Value)
                .AverageAsync() ?? 0.0;
        }
        public async Task<int> GetRatingCount(int gameId)
        {
            return await _context.Ratings
                .Where(r => r.GameId == gameId)
                .CountAsync();
        }

    }
}
