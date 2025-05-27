using GameZone.Models;
using GameZone.ViewModel;

namespace GameZone.Services
{
    public interface IGamesServices
    {
        IEnumerable<Games> GetAll();
        int Count();
        Games? GetById(int id);
        Task Create(CreateGameViewModel game);
        Task<Games>? Update(EditGameViewModel game);
        bool Delete(int id);
    }
}
