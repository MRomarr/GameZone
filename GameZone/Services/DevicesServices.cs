using GameZone.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly ApplicationDbContext _context;
        public DevicesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetSelectLists()
        {
            return _context.Devices.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).OrderBy(c => c.Text).AsNoTracking()
            .ToList();
        }
    }
}
