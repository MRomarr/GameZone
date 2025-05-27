using GameZone.Models;
using GameZone.Services;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

public class CartService : ICartService
{
    private const string CartCookieKey = "CartCookie";
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGamesServices _gamesServices;

    public CartService(IHttpContextAccessor httpContextAccessor, IGamesServices gamesServices)
    {
        _httpContextAccessor = httpContextAccessor;
        _gamesServices = gamesServices;
    }

    public List<Games> GetCart()
    {
        var cartIds = GetCartIds();
        var cart = new List<Games>();
        foreach (var id in cartIds)
        {
            var game = _gamesServices.GetById(id);
            if (game != null)
                cart.Add(game);
        }
        return cart;
    }

    public List<int> GetCartIds()
    {
        var request = _httpContextAccessor.HttpContext.Request;
        var cartJson = request.Cookies[CartCookieKey];
        return string.IsNullOrEmpty(cartJson)
            ? new List<int>()
            : JsonSerializer.Deserialize<List<int>>(cartJson);
    }

    public void AddToCart(int gameId)
    {
        var cartIds = GetCartIds();
        if (!cartIds.Contains(gameId))
        {
            cartIds.Add(gameId);
            SaveCartIds(cartIds);
        }
    }

    public void RemoveFromCart(int gameId)
    {
        var cartIds = GetCartIds();
        if (cartIds.Contains(gameId))
        {
            cartIds.Remove(gameId);
            SaveCartIds(cartIds);
        }
    }

    public void ClearCart()
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(CartCookieKey);
    }

    private void SaveCartIds(List<int> gameIds)
    {
        var response = _httpContextAccessor.HttpContext.Response;
        var cartJson = JsonSerializer.Serialize(gameIds);
        response.Cookies.Append(CartCookieKey, cartJson, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true
        });
    }
}
