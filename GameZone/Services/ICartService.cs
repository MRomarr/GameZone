using GameZone.Models;
public interface ICartService
{
    List<Games> GetCart();
    void AddToCart(int gameId);
    void RemoveFromCart(int gameId);
    void ClearCart();
    List<int> GetCartIds();
}
