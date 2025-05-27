using System.Diagnostics;
using System.Net;
using GameZone.Data;
using GameZone.Models;
using GameZone.Services;
using GameZone.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace GameZone.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGamesServices _gamesServices;
        private readonly ICategoriesServics _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRatingServies _ratingServies;

        public HomeController(IGamesServices gamesServices,ICategoriesServics categoriesService,UserManager<ApplicationUser> userManager,IRatingServies ratingServies,IDevicesService devicesService)
        {
            _gamesServices = gamesServices;
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _userManager = userManager;
            _ratingServies = ratingServies;
        }

        public IActionResult Index(int? Devices, int? Categories, string? search, int page = 1)
        {

            int pageSize = 10;   // how many games want in one page

            var games = _gamesServices.GetAll();


            if (Devices != null)    // check if the user select a device
                games = games.Where(g => g.Devices.Any(d => d.DeviceId == Devices));

            if (Categories != null)    // check if the user select a category
                games = games.Where(g => g.CategoryId == Categories);



            if (!string.IsNullOrEmpty(search))   // check if the user search for a game
                games = games.Where(g => g.Name.Contains(search, StringComparison.OrdinalIgnoreCase));


            var totalGames = games.Count();   // count the games to culc totalpages
            var totalPages = (int)Math.Ceiling((double)totalGames / pageSize); // calculate total pages for pagination


            games = games.OrderBy(g => g.Id).Skip((page - 1) * pageSize).Take(pageSize);


            var viewModel = new GamesIndexViewModel
            {
                Games = games.ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectLists(),
                CurrentPage = page,
                TotalPages = totalPages
            };
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int GameId, int value)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();
            var Rating= _ratingServies.GetUserRating(GameId,user.Id);
            if (Rating != null)
            {
               await _ratingServies.RemoveRating(GameId, user.Id); // Remove existing rating if it exists
            }
            if (value < 1 || value > 5)
                return BadRequest("Invalid rating value");

            await _ratingServies.AddRating(GameId, user.Id, value);

            return Json(new { success = true });
        }


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


        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var game = _gamesServices.GetById(id);
            var rating = _ratingServies.GetUserRating(id, user?.Id);
            ViewBag.Rating = rating?.Value ?? 0; 
            var avreageRating = await _ratingServies.GetAverageRating(id);
            ViewBag.AverageRating = avreageRating;
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
