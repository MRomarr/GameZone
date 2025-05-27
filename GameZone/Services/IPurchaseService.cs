namespace GameZone.Services
{
    public interface IPurchaseService
    {
        Task PurchaseGamesAsync(string userId, List<int> gameIds);
    }
}
