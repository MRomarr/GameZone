using System.ComponentModel.DataAnnotations;
using GameZone.Attributes;
using GameZone.Settings;

namespace GameZone.ViewModel
{
    public class EditGameViewModel: GameFormViewModel
    {

        public int Id { get; set; }
        public string? CurrentCover { get; set; } 
        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSizeAttibute(FileSettings.MaxFileSizeByte)]
        public IFormFile? Cover { get; set; } = default;
    }
}
