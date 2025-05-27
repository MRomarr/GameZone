using System.Threading.Tasks;
using GameZone.Data;
using GameZone.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
   // [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        public RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> RoleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = RoleManager;
            _userManager = userManager;
        }
        
        
        
        
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles); 
        }
        
        
        
        
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]  
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Result = await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
                if(Result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }








        public async Task<IActionResult> Delete(string RoleName)
        {
            var role = await _roleManager.FindByNameAsync(RoleName);
            if (role == null)
                return NotFound("Role not found");

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded ? RedirectToAction("Index") : BadRequest(result.Errors);
        }


    }
}
