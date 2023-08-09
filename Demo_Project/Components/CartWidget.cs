using Demo_Project.Data;
using Demo_Project.Infrastructure;
using Demo_Project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Project.Components
{
    public class CartWidget: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetJson<Cart>("cart"));
        }
    }
}
