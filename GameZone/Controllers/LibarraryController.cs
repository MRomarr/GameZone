using GameZone.Data;
using GameZone.Models;
using GameZone.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Controllers
{
    
    public class LibarraryController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserGamesServies _userGamesServies;

        public LibarraryController(UserManager<ApplicationUser> userManager, IUserGamesServies userGamesServies)
        {
            _userManager = userManager;
            _userGamesServies = userGamesServies;
        }


        // all games the player have --------
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login","Acount");
            }

            var games = _userGamesServies.GetAll(user.Id) ;

            return View(games);
        }





    }
}
