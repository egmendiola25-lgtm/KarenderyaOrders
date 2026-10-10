using KarenderyaOrders.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KarenderyaOrders.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

    public IActionResult Index(string searchString, string status, int page = 1)
        {
            int pageSize = 5;

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                orders = orders.Where(o =>
                    o.OrderNumber.Contains(searchString));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                orders = orders.Where(o => o.Status == status);
            }

            int totalOrders = orders.Count();

            var pagedOrders = orders
                .OrderByDescending(o => o.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.SearchString = searchString;
            ViewBag.SelectedStatus = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalOrders / (double)pageSize);

            return View(pagedOrders);
        }

        public IActionResult Details(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var foodItems = _context.FoodItems.ToList();

            ViewBag.FoodItems = foodItems;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(
            KarenderyaOrders.Models.Order order,
            int FoodItemId,
            int Quantity)
        {
            if (ModelState.IsValid)
            {
                bool orderNumberExists = _context.Orders
                    .Any(o => o.OrderNumber == order.OrderNumber);

                if (orderNumberExists)
                {
                    ModelState.AddModelError(
                        "OrderNumber",
                        "This order number already exists.");

                    ViewBag.FoodItems = _context.FoodItems.ToList();
                    return View(order);
                }

                var foodItem = _context.FoodItems.Find(FoodItemId);

                if (foodItem == null || Quantity <= 0 || foodItem.StockQuantity < Quantity)
                {
                    ModelState.AddModelError("", "Please select a valid food item, enter a positive quantity, and ensure enough stock is available.");
                    ViewBag.FoodItems = _context.FoodItems.ToList();
                    return View(order);
                }

                order.CreatedAt = DateTime.Now;
                order.TotalAmount = foodItem.Price * Quantity;

                _context.Orders.Add(order);
                _context.SaveChanges();

                var orderItem = new KarenderyaOrders.Models.OrderItem
                {
                    OrderId = order.Id,
                    FoodItemId = FoodItemId,
                    Quantity = Quantity,
                    UnitPrice = foodItem.Price,
                    Subtotal = foodItem.Price * Quantity
                };
                _context.OrderItems.Add(orderItem);

                foodItem.StockQuantity -= Quantity;
                _context.FoodItems.Update(foodItem);

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.FoodItems = _context.FoodItems.ToList();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Complete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            order.Status = "Completed";
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.FoodItem)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            if (order.Status == "Cancelled" || order.Status == "Completed")
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var item in order.OrderItems)
            {
                if (item.FoodItem != null)
                {
                    item.FoodItem.StockQuantity += item.Quantity;
                }
            }

            order.Status = "Cancelled";
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}