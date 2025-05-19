using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using GameZone.Attributes;
using GameZone.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.ViewModel
{
    
    public class CreateGameViewModel: GameFormViewModel
    {
        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSizeAttibute(FileSettings.MaxFileSizeByte)]
        public IFormFile Cover { get; set; } = default;
    }
}
