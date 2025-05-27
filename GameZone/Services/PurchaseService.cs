namespace GameZone.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IUserGamesServies _userGamesService;

        public PurchaseService(IUserGamesServies userGamesService)
        {
            _userGamesService = userGamesService;
        }

        public async Task PurchaseGamesAsync(string userId, List<int> gameIds)
        {
            foreach (var gameId in gameIds)
            {
                if (_userGamesService.GetById(userId, gameId) == null)
                {
                    _userGamesService.Add(userId, gameId);
                }
            }

            
        }
    }

}
