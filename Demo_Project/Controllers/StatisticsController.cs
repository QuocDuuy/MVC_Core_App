using Demo_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Demo_Project.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var orders = _context.OrderDetail.Include(o => o.Product).ToList();

            // Logic thống kê dữ liệu từ đơn hàng
            var totalRevenue = orders.Sum(order => order.total_price);
            var totalOrders = orders.Count;

            // Thống kê mặt hàng được đặt nhiều nhất và ít nhất
            var productStatistics = orders
                .GroupBy(order => order.Product.ProductName)
                .Select(group => new
                {
                    ProductName = group.Key,
                    TotalOrders = group.Count()
                })
                .OrderByDescending(stat => stat.TotalOrders)
                .ToList();

            string mostOrderedProduct = productStatistics.FirstOrDefault()?.ProductName ?? "N/A";
            string leastOrderedProduct = productStatistics.LastOrDefault()?.ProductName ?? "N/A";

            // Thống kê số lượng sản phẩm trong trạng thái "Chờ thanh toán" và "Đã thanh toán"
            var productsInPendingStatus = orders.Count(order => order.order_status == "Chờ thanh toán");
            var productsInPaidStatus = orders.Count(order => order.order_status == "Đã thanh toán");

            // Truyền dữ liệu thống kê đến view
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.MostOrderedProduct = mostOrderedProduct;
            ViewBag.LeastOrderedProduct = leastOrderedProduct;
            ViewBag.ProductsInPendingStatus = productsInPendingStatus;
            ViewBag.ProductsInPaidStatus = productsInPaidStatus;
            
            // Logic thống kê dữ liệu từ đơn hàng
            var monthlyRevenue = new decimal[12]; // Mảng lưu tổng doanh thu từng tháng, mảng bắt đầu từ tháng 1

            foreach (var order in orders)
            {
                int month = order.cus_date.Month - 1; // Chuyển sang chỉ số tháng (0-11)
                monthlyRevenue[month] += order.total_price;
            }

            // Truyền dữ liệu thống kê đến view
            ViewBag.MonthlyRevenue = monthlyRevenue;


            // THỐNG KÊ TỔNG SỐ LƯỢNG SẢN PHẨM BÁN ĐƯỢC THEO TỪNG THÁNG
            var monthlyProductCounts = new int[12]; // Mảng lưu số lượng sản phẩm bán được từng tháng, mảng bắt đầu từ tháng 1

            foreach (var order in orders)
            {
                int month = order.cus_date.Month - 1; // Chuyển sang chỉ số tháng (0-11)
                monthlyProductCounts[month] += order.quantity;
            }

            // Truyền dữ liệu thống kê đến view
            ViewBag.MonthlyProductCounts = monthlyProductCounts;
            return View(orders);
        }

    }
}
