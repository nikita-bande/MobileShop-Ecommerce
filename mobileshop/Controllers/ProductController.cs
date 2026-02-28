using mobileshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace mobileshop.Controllers
{
    public class ProductController : Controller
    {
        MobileShopDBContext db = new MobileShopDBContext();

        // Home Page Index with Search and Sorting
        public ActionResult Index(string searchString, string sortOrder)
        {
            var products = db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString) || p.Category.Contains(searchString));
            }

            ViewBag.PriceSortParam = sortOrder == "Price_Desc" ? "Price_Asc" : "Price_Desc";

            switch (sortOrder)
            {
                case "Price_Desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "Price_Asc":
                    products = products.OrderBy(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }
            return View(products.ToList());
        }

        // Product Details with Related Products and Dynamic Reviews
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var reviews = db.Reviews
                            .Where(r => r.ProductId == id)
                            .OrderByDescending(r => r.ReviewDate)
                            .ToList();

            ViewBag.Reviews = reviews;

            // 2. Same category products (Related Items) fetch 
            var relatedProducts = db.Products
                                    .Where(p => p.Category == product.Category && p.Id != id)
                                    .Take(4)
                                    .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            return View(product);
        }

        // Action to Post a New Review
        [HttpPost]
        public ActionResult PostReview(int ProductId, string Comment)
        {
            if (Session["UserName"] != null && !string.IsNullOrEmpty(Comment))
            {
                Review newReview = new Review
                {
                    ProductId = ProductId,
                    UserName = Session["UserName"].ToString(),
                    Comment = Comment,
                    ReviewDate = DateTime.Now
                };

                db.Reviews.Add(newReview);
                db.SaveChanges(); 
            }
            return RedirectToAction("Details", new { id = ProductId });
        }
    }
}