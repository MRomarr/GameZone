using System.Threading.Tasks;
using GameZone.Data;
using GameZone.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            
        }
        public async Task<IActionResult> Details()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");
            ProfileViewModel UserProfile = new()
            {
                UserName = user.UserName,
                Email = user.Email,
                Password= "********" // Password should not be displayed
            };
            return View(UserProfile);
        }
    }
}
