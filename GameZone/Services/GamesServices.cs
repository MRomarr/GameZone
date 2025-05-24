using System.Threading.Tasks;
using GameZone.Data;
using GameZone.Models;
using GameZone.Settings;
using GameZone.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class GamesServices : IGamesServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _coverPath;
        public GamesServices(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _coverPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }
        public IEnumerable<Games> GetAll()
        {
            return _context.Games.Include(g=>g.Category).Include(g=>g.Devices).ThenInclude(d => d.Device)
                .AsNoTracking().ToList(); 
        }
        public Games? GetById(int id)
        {
            var game =  _context.Games.Include(g => g.Category).Include(g => g.Devices).ThenInclude(d => d.Device)
                .AsNoTracking().SingleOrDefault(g => g.Id == id);
            return game;
        }

        public async Task Create(CreateGameViewModel model)
        {
            var conver = await SaveCover(model.Cover);
            Games newGame = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = conver,
                price = model.price,
                Devices = model.SelectedDiveces.Select(d => new GameDevice
                {
                    DeviceId = d 
                }).ToList()
            };
            _context.Add(newGame);
            _context.SaveChanges();
        }
        public async Task<Games>? Update(EditGameViewModel model)
        {
            var game = _context.Games.Include(g=>g.Devices).SingleOrDefault(g=>g.Id==model.Id);
            if (game is null) return null;
            var hasnewCover = model.Cover != null;
            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.price = model.price;
            game.Devices = model.SelectedDiveces.Select(d => new GameDevice
            {
                DeviceId = d
            }).ToList();
            if (hasnewCover)
                game.Cover = await SaveCover(model.Cover);
            var effectedRow = _context.SaveChanges();
            if (effectedRow > 0)
            {
                if (hasnewCover)
                {
                    var coverPath = Path.Combine(_coverPath, model.CurrentCover); // Fix: Ensure oldCover is a string
                    if (System.IO.File.Exists(coverPath))
                    {
                        System.IO.File.Delete(coverPath);
                    }
                }
            }
            
            return game;
        }
        private async Task<string> SaveCover(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_coverPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.Find(id);

            if (game is null)
                return isDeleted;

            _context.Remove(game);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;

                var cover = Path.Combine(_coverPath, game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }
    }
}
