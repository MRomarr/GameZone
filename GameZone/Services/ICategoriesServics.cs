using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Services
{
    public interface ICategoriesServics
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
