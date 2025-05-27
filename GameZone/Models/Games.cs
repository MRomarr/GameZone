using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Games: BaseEntity
    {
       
        public string Description { get; set; }
        public string Cover { get; set; }
        public double price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<GameDevice> Devices { get; set; }
        public UserGames UserGames { get; set; }

    }
}
