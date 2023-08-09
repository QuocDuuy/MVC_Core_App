using Demo_Project.Data;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Project.Components
{
    public class Imagebar: ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Imagebar(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View("Index",_context.Categories.ToList());
        }
    }
}
