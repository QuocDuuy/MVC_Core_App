using Demo_Project.Data;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Project.Components
{
    public class Trendy: ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public Trendy(ApplicationDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Products.Where(p => p.isTrendy==true).ToList());
        }
    }
}
