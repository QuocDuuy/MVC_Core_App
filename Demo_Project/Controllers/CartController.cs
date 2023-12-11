using Demo_Project.Data;
using Demo_Project.Infrastructure;
using Demo_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo_Project.Controllers
{
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(Cart);
            //return View("Cart", HttpContext.Session.GetJson<Cart>("cart"));
        }
        const string KeyCartName = "CART";
        public List<CartLine> CartLines { get; set; } 
        

        public IActionResult AddToCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, 1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            else
            {
                product = new Product();
            } 
                
            return View("Cart", Cart);
        }

        public IActionResult UpdateCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, -1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }

        public IActionResult Checkout()
        {
            // Implement your checkout logic here, such as saving order details to the database,
            // calculating payment, sending emails, etc.

            // For demonstration purposes, let's assume the checkout was successful:
            ViewBag.CheckoutMessage = "Your order has been successfully placed!";

            // Return a view with a confirmation message
            return RedirectToAction("Create", "Order");
        }

        public IActionResult RemoveFromCart(int productId)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }
    }
}   

