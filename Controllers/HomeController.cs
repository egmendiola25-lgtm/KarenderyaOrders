using KarenderyaOrders.Models;
using KarenderyaOrders.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KarenderyaOrders.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalOrders = _context.Orders.Count();

            ViewBag.TodaySales = _context.Orders
                .Where(o => o.Status == "Completed" &&
                            o.CreatedAt.Date == DateTime.Today)
                .Sum(o => (decimal?)o.TotalAmount) ?? 0;

            var lowStockItems = _context.FoodItems
                .Where(f => f.StockQuantity <= 5)
                .ToList();

            ViewBag.LowStockItems = lowStockItems;
            ViewBag.LowStockCount = lowStockItems.Count;

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
