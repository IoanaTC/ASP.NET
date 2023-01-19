using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelMare.Models;

namespace ModelMare.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext db;
        public ProductsController(AppDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var products = db.Products.Include("Category");
            // afisare paginata
            int _perPage = 3;

            int totalItems = products.Count();

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            int offset = 0;
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }

            var paginatedProducts = products.Skip(offset).Take(_perPage);

            ViewBag.lastPage = Math.Ceiling((float)totalItems / (float)_perPage);
            ViewBag.products = paginatedProducts;

            if(TempData.ContainsKey("message"))
                ViewBag.message = TempData["message"];

            return View();
        }
    }
}
