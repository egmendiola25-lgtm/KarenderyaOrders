
using KarenderyaOrders.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KarenderyaOrders.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var foodItems = _context.FoodItems.ToList();
            return View(foodItems);
        }

        // Displays the form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Saves the submitted food item
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KarenderyaOrders.Models.FoodItem foodItem)
        {
            if (ModelState.IsValid)
            {
                _context.FoodItems.Add(foodItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(foodItem);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var foodItem = _context.FoodItems.Find(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, KarenderyaOrders.Models.FoodItem foodItem)
        {
            if (id != foodItem.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.FoodItems.Update(foodItem);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(foodItem);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var foodItem = _context.FoodItems.Find(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var foodItem = _context.FoodItems.Find(id);

            if (foodItem == null)
            {
                return NotFound();
            }

            _context.FoodItems.Remove(foodItem);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


    }
}
