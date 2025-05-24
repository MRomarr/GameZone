using System.Diagnostics;
using GameZone.Models;
using GameZone.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGamesServices _gamesServices;
        private readonly ICategoriesServics _categoriesService;
        private readonly IDevicesService _devicesService;

        public HomeController(
            IGamesServices gamesServices,
            ICategoriesServics categoriesService,
            IDevicesService devicesService)
        {
            _gamesServices = gamesServices;
            _categoriesService = categoriesService;
            _devicesService = devicesService;
        }

        public IActionResult Index(int? Devices, int? Categories, string? search)
        {
            ViewBag.Categories = _categoriesService.GetSelectList();
            ViewBag.Devices = _devicesService.GetSelectLists();
            //ViewBag.Categories = new SelectList(_categoriesService.GetSelectList(), "Id", "Name", Categories);
            //ViewBag.Devices = new SelectList(_devicesService.GetSelectLists(), "Id", "Name", Devices);

            var games = _gamesServices.GetAll();

            if (Devices != null)
                games = games.Where(g => g.Devices.Any(d => d.DeviceId == Devices)).ToList();

            if (Categories != null)
                games = games.Where(g => g.CategoryId == Categories).ToList();

            if (!string.IsNullOrEmpty(search))
                games = games.Where(g => g.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return View(games);
        }

        [HttpGet]
        [HttpGet]
        public JsonResult SearchGames(string term)
        {
            var results = _gamesServices.GetAll()
                .Where(g => g.Name.ToLower().Contains(term.ToLower()))
                .Select(g => new {
                    id = g.Id,
                    label = g.Name
                })
                .ToList();
            return Json(results);
        }


        public IActionResult Details(int id)
        {
            var game = _gamesServices.GetById(id);

            if (game is null)
                return NotFound();

            return View(game);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
