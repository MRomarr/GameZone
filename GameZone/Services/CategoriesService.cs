﻿using GameZone.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class CategoriesService : ICategoriesServics
    {
        private readonly ApplicationDbContext _context;
        public CategoriesService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return _context.Categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).OrderBy(c => c.Text).AsNoTracking().ToList();
        }
    }
}
