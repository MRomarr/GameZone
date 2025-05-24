using GameZone.Models;
using GameZone.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using GameZone.Data;

namespace GameZone.Controllers
{
    public class CartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserGamesServies _userGamesServies;
        private readonly IGamesServices _gamesServices;

        public CartController(UserManager<ApplicationUser> userManager, IUserGamesServies userGamesServies, IGamesServices gamesServices)
        {
            _userManager = userManager;
            _userGamesServies = userGamesServies;
            _gamesServices = gamesServices;
        }
        public IActionResult Index()
        {
            var cart = GetCartFromSession();
            return View(cart);
        }
        private const string CartSessionKey = "Cart";
        public IActionResult Add(int id)
        {
            var game = _gamesServices.GetById(id);
            var cart = GetCartFromSession();
            if (!cart.Any(g => g.Id == id))
            {
                cart.Add(game);
            }
            else
            {
                return Json(new { success = true, alredyadded = true, count = cart.Count });
            }

            SaveCartToSession(cart);
            return Json(new { success = true, alredyadded= false , count = cart.Count });
        }
        public IActionResult Remove(int id)
        {
            var cart = GetCartFromSession();
            var gameToRemove = cart.FirstOrDefault(g => g.Id == id);
            if (gameToRemove != null)
            {
                cart.Remove(gameToRemove);
                SaveCartToSession(cart);
            }
            return RedirectToAction("Index");
        }
        private List<Games> GetCartFromSession()
        {
            var cartJson = HttpContext.Session.GetString(CartSessionKey);
            return cartJson == null ? new List<Games>() : JsonSerializer.Deserialize<List<Games>>(cartJson);
        }
        private void SaveCartToSession(List<Games> cart)
        {
            // To avoid possible reference loop issues during serialization, configure JsonSerializerOptions
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            var cartJson = JsonSerializer.Serialize(cart, options);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }
        
        public async Task<IActionResult> BuyAllCart()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = GetCartFromSession();
            if (user != null)
            {
                foreach (var game in cart)
                {
                    if (_userGamesServies.GetById(user.Id, game.Id) == null)
                    {
                        _userGamesServies.Add(user.Id, game.Id);
                    }
                }
            }
            return RedirectToAction("Index", "Libarrary");

        }
        
    }
}
