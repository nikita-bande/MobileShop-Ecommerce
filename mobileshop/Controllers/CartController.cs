using mobileshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace mobileshop.Controllers
{
    public class CartController : Controller
    {
        MobileShopDBContext db = new MobileShopDBContext();

        // 1. Add Product to Cart
        public ActionResult AddToCart(int id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null) return HttpNotFound();

            List<Product> cart = (List<Product>)Session["cart"] ?? new List<Product>();
            cart.Add(product);
            Session["cart"] = cart;

            return RedirectToAction("Index", "Product");
        }

        // 2. View Cart Items
        public ActionResult ViewCart()
        {
            List<Product> cart = (List<Product>)Session["cart"] ?? new List<Product>();
            return View(cart);
        }

        // 3. Remove Product from Cart
        public ActionResult RemoveFromCart(int id)
        {
            List<Product> cart = (List<Product>)Session["cart"];
            if (cart != null)
            {
                var itemToRemove = cart.FirstOrDefault(p => p.Id == id);
                if (itemToRemove != null) cart.Remove(itemToRemove);
                Session["cart"] = cart;
            }
            return RedirectToAction("ViewCart");
        }

        // 4. Checkout Page 
        [HttpGet]
        public ActionResult Checkout()
        {
            var cart = (List<Product>)Session["cart"];
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Product");
            }
            return View();
        }

       
        [HttpPost]
        public ActionResult PlaceOrder(Order order)
        {
            var cart = (List<Product>)Session["cart"];
            if (cart == null || !cart.Any()) return RedirectToAction("Index", "Product");

            if (Session["UserId"] != null)
            {
                order.UserId = Convert.ToInt32(Session["UserId"]);
            }
            else
            {
       
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.TotalAmount = cart.Sum(p => (decimal)p.Price);
                order.Status = "Pending";

                db.Orders.Add(order);
                db.SaveChanges(); 

                Session["cart"] = null;
                return RedirectToAction("OrderSuccess");
            }
            return View("Checkout", order);
        }
        // 6. Success Page
        public ActionResult OrderSuccess()
        {
            return View();
        }
    }
}