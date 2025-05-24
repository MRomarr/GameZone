using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameZone.ViewModel
{
    public class GameFormViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        
        public Double price { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        [Display(Name = "Supported Devices")]
        public List<int> SelectedDiveces { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> Devices { get; set; } = new List<SelectListItem>();

        public string Description { get; set; }
    }
}
