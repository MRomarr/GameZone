using GameZone.Data;
using GameZone.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class UserGamesServies : IUserGamesServies
    {
        private readonly ApplicationDbContext _context;

        public UserGamesServies(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Games> GetAll(string id)
        {
            return _context.Games
                .Include(g => g.UserGames).Include(g => g.Category).Include(d => d.Devices)
                .Where(g => g.UserGames != null && g.UserGames.UserId == id)
                .ToList();
        }
        public Games GetById(string Userid,int gameId)
        {
            return _context.Games
                .Include(g => g.UserGames).Include(g => g.Category).Include(d => d.Devices)
                .FirstOrDefault(g => g.UserGames != null && g.UserGames.UserId == Userid && g.UserGames.GameId == gameId);
                
        }
        public void Add(string UserId,int GameId)
        {
            UserGames userGames = new()
            {
                UserId = UserId,
                GameId= GameId
            };

            _context.UserGames.Add(userGames);
            _context.SaveChanges();
        }
    }
}
