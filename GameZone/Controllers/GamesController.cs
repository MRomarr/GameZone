using GameZone.Services;
using GameZone.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers;
[Authorize] 
// will add admin here
// this is the admin port ---------------------------------------------
public class GamesController : Controller
{
    private readonly ICategoriesServics _categoriesService;
    private readonly IDevicesService _devicesService;
    private readonly IGamesServices _gamesService;

    public GamesController(ICategoriesServics categoriesService,IDevicesService devicesService,IGamesServices gamesService)
    {
        _categoriesService = categoriesService;
        _devicesService = devicesService;
        _gamesService = gamesService;
    }

    // all games ---------------------------------
    public IActionResult Index()
    {
        var games = _gamesService.GetAll();
        return View(games);
    }




    // game detail ------------------------------

    [HttpGet]
    public IActionResult Details(int id)
    {
        var game = _gamesService.GetById(id);

        if (game is null)
            return NotFound();

        return View(game);
    }   // wiill change this





    // create game --------------------------------
    public IActionResult Create()
    {
        CreateGameViewModel viewModel = new()
        {
            Categories = _categoriesService.GetSelectList(),
            Devices = _devicesService.GetSelectLists()
        };

        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateGameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _categoriesService.GetSelectList();
            model.Devices = _devicesService.GetSelectLists();
            return View(model);
        }

        await _gamesService.Create(model);

        return RedirectToAction(nameof(Index));
    }




    // edit game -----------------------------------
    public IActionResult Edit(int id)
    {
        var game = _gamesService.GetById(id);

        if (game is null)
            return NotFound();

        EditGameViewModel viewModel = new()
        {
            Id = id,
            Name = game.Name,
            Description = game.Description,
            CategoryId = game.CategoryId,
            SelectedDiveces = game.Devices.Select(d => d.DeviceId).ToList(),
            Categories = _categoriesService.GetSelectList(),
            Devices = _devicesService.GetSelectLists(),
            CurrentCover = game.Cover,
            price = game.price

        };

        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditGameViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _categoriesService.GetSelectList();
            model.Devices = _devicesService.GetSelectLists();
            return View(model);
        }

        var game = await _gamesService.Update(model);
            
        if (game is null)
            return BadRequest();

        return RedirectToAction(nameof(Index));
    }





    // delete game -----------------------------------
    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var isDeleted = _gamesService.Delete(id);
        return isDeleted ? Ok() : BadRequest();
    }
    
}