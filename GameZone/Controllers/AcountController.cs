using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using GameZone.Data;
using GameZone.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class AcountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
       
        public AcountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel NewUserViewModel)
        {

            if (ModelState.IsValid)
            {
                var token = Guid.NewGuid().ToString();
                var user = new ApplicationUser
                {
                    UserName = NewUserViewModel.UserName,
                    Email = NewUserViewModel.Email,
                    
                    PasswordHash = NewUserViewModel.Password,
                   
                };
                var result = await _userManager.CreateAsync(user, NewUserViewModel.Password);
                 if (result.Succeeded)
                {

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // if want to add role 
                    await _userManager.AddToRoleAsync(user, "Admin");
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(NewUserViewModel);
        }

        

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (ModelState.IsValid)
            {
                
                var user = await _userManager.FindByEmailAsync(loginUserViewModel.UserNameOrEmail);
                
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(loginUserViewModel.UserNameOrEmail);
                }
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(loginUserViewModel);
                }
               

                var result = await _signInManager.PasswordSignInAsync(user.UserName, loginUserViewModel.Password,isPersistent: loginUserViewModel.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid login attempt.");
                return View(loginUserViewModel);
            }
            return View(loginUserViewModel);
        }
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Acount");
            
        }
    }
}
