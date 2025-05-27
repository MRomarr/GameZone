using System.ComponentModel.DataAnnotations;
using GameZone.Data;

namespace GameZone.Models
{
    public class Rating
    {
        [Range(1, 5)]
        public int Value { get; set; }

        public int GameId { get; set; }
        public Games Game { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } 
    }

}
