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
        



        // Register --------------
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



        
        // login ------------------
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




        // change password form ---------
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Acount");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        





        // forget pasword ------------







        // logout ------------------
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Acount");
            
        }
    }
}
