using GameZone.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.ViewModel
{
    public class GamesIndexViewModel
    {
        public IEnumerable<Games> Games { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Devices { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
