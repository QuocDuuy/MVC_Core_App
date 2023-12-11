using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Demo_Project.Data;
using Demo_Project.Infrastructure;
using Demo_Project.Models;
using System.Data;

namespace Demo_Project.Controllers
{
    public class OrderInforController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderInforController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orderInfors = await _context.OrderInfor.ToListAsync();

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
            }

            return View(orderInfors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,email,phone,cus_address")] OrderInfor orderInfor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderInfor);
                await _context.SaveChangesAsync();

                // Assuming the order is successfully placed
                TempData["SuccessMessage"] = "Order Successfully";

                // Create OrderDetail entries based on Cart items
                Cart cart = HttpContext.Session.GetJson<Cart>("cart");
                if (cart != null)
                {
                    decimal total = 0;

                    foreach (var line in cart.Lines)
                    {
                        var product = line.Product;

                        _context.OrderDetail.Add(new OrderDetail
                        {
                            product_id = product.ProductId,
                            orderId = orderInfor.order_id,
                            total_price = (decimal)(product.ProductPrice * (1 - product.ProductDiscount) * line.Quantity),
                            quantity = line.Quantity,
                            order_status = "Chờ thanh toán"
                        });

                        total += (decimal)(product.ProductPrice * (1 - product.ProductDiscount) * line.Quantity);
                    }

                    await _context.SaveChangesAsync();

                    // Convert the total to string before storing in TempData
                    TempData["TotalPrice"] = total.ToString();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(orderInfor);
        }
    }
}
