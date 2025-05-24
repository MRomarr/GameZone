using GameZone.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Services
{
    public interface IUserGamesServies
    {
        List<Games> GetAll(string id);
        public void Add(string UserId, int GameId);
        public Games GetById(string Userid, int gameId);
    }
}
