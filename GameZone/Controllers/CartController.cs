using GameZone.Data;
using GameZone.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class CartController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserGamesServies _userGamesServies;
    private readonly ICartService _cartService;
    private readonly IPurchaseService _purchaseService;

    public CartController(UserManager<ApplicationUser> userManager,IUserGamesServies userGamesServies,ICartService cartService,IPurchaseService purchaseService)
    {
        _userManager = userManager;
        _userGamesServies = userGamesServies;
        _cartService = cartService;
        _purchaseService = purchaseService;
    }




    // all cart item -----------------------
    public IActionResult Index()
    {
        var cart = _cartService.GetCart();
        double total = 0;
        foreach(var item in cart)
        {
            total += item.price;
        }
        ViewBag.TotalPrice = total;
        return View(cart);
    }





    // add item to cart --------------------
    public async Task<IActionResult> Add(int id)
    {
        var cart = _cartService.GetCart();
        var user = await _userManager.GetUserAsync(User);

        if (user != null && _userGamesServies.GetById(user.Id, id) != null)
        {
            // User already owns the game
            return Json(new { success = false, alreadyInCart = false, alreadyOwned = true, count = cart.Count });
        }

        if (cart.Any(g => g.Id == id))
        {
            // Game already in cart
            return Json(new { success = false, alreadyInCart = true, alreadyOwned = false, count = cart.Count });
        }

        // Add to cart
        _cartService.AddToCart(id);
        cart = _cartService.GetCart(); // refresh to get updated count

        return Json(new { success = true, alreadyInCart = false, alreadyOwned = false, count = cart.Count });
    }





    // remove item from cart -----------------
    public IActionResult Remove(int id)
    {
        _cartService.RemoveFromCart(id);
        return RedirectToAction("Index");
    }





    // buy item from cart of home directuly --------
    public async Task<IActionResult> Buy(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            if (_userGamesServies.GetById(user.Id, id) == null)
            {
                _userGamesServies.Add(user.Id, id);
                _cartService.RemoveFromCart(id);
                return RedirectToAction("Index", "Libarrary");
            }
        }
        return RedirectToAction("Login", "Acount");

    }





    // buy all items in cart -----------------
    public async Task<IActionResult> BuyAllCart()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null)
        {
            var gameIds = _cartService.GetCartIds();
            await _purchaseService.PurchaseGamesAsync(user.Id, gameIds);
            _cartService.ClearCart();
        }

        return RedirectToAction("Index", "Libarrary");
    }


    
}
