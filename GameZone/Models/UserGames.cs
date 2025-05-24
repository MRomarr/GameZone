using GameZone.Data;

namespace GameZone.Models
{
    public class UserGames
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int GameId { get; set; }
        public Games Game { get; set; }
    }
}
