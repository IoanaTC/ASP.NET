using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ModelMare.Models;
using System.ComponentModel.DataAnnotations.Schema;

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
        public IActionResult Show(int? id)
        {
            var product = db.Products.Include("Category").Where(x => x.Id == id).First();

            if (TempData.ContainsKey("message"))
                ViewBag.message = TempData["message"];

            return View(product);
        }
        public IActionResult New()
        {
            Product product = new Product();
            product.Categories = GetAllCategories();

            return View(product);
        }
        [HttpPost]
        public IActionResult New(Product requestProduct)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(requestProduct);
                db.SaveChanges();

                TempData["message"] = "Produsul a fost adaugat";
                return RedirectToAction("Index");
            }
            else
            {
                requestProduct.Categories = GetAllCategories();
                return View(requestProduct);
            }
        }
        public IActionResult Edit(int? id)
        {
            var product = db.Products.Find((int)id);

            product.Categories = GetAllCategories();

            return View(product);
        }
        [HttpPost]
        public IActionResult Edit(int? id, Product requestProduct)
        {
            var product = db.Products.Include("Category").Where(x => x.Id == id).First();
            if (ModelState.IsValid)
            {
                product.Denumire = requestProduct.Denumire;
                product.Descriere = requestProduct.Descriere;
                product.DateExp = requestProduct.DateExp;

                db.SaveChanges();
                TempData["message"] = "Produsul a fost modificat";
                return Redirect("/Products/Show/" + (int)id);
            }
            else
            {
                requestProduct.Categories = GetAllCategories();
                return View(requestProduct);
            }
        }
        public IActionResult Delete(int? id)
        {
            var product = db.Products.Where(x => x.Id == id).First();
            db.Products.Remove(product);

            db.SaveChanges();
            TempData["message"] = "Produsul a fost sters";
            return Redirect("/Products/Index");
        }
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = db.Categories;

            foreach (var category in categories)
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Nume.ToString()
                });

            return selectList;
        }
    }
}
