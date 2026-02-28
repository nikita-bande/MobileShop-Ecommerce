using mobileshop.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web;
using Rotativa;
using System.IO;
using System;
using System.Collections.Generic;

namespace mobileshop.Controllers
{
    public class AdminController : Controller
    {
        MobileShopDBContext db = new MobileShopDBContext();

        // 1. Products Page (Admin side list) -
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products); 
        }

        // 2. Orders Page (Admin side list) -
        public ActionResult Orders()
        {
            var orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders); 
        }

        public ActionResult Dashboard()
        {
            ViewBag.TotalOrders = db.Orders.Count();
            ViewBag.TotalRevenue = db.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;
            ViewBag.TotalProducts = db.Products.Count();
            ViewBag.TotalUsers = db.Users.Count();

            var lowStockProducts = db.Products
                                     .Where(p => p.Quantity < 5)
                                     .ToList();
            // Out of Stock Logic: Quantity 0 
            ViewBag.OutOfStockCount = db.Products.Count(p => p.Quantity == 0);

            ViewBag.LowStock = lowStockProducts;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null && uploadImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(uploadImage.FileName);
                string folderPath = Server.MapPath("~/Content/images/");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                string fullPath = Path.Combine(folderPath, fileName);
                uploadImage.SaveAs(fullPath);
                product.Image = "~/Content/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductName,Category,Price,Description,Image,Quantity")] Product product, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null && uploadImage.ContentLength > 0)
            {
                string fileName = Path.GetFileName(uploadImage.FileName);
                string folderPath = Server.MapPath("~/Content/images/");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
                string fullPath = Path.Combine(folderPath, fileName);
                uploadImage.SaveAs(fullPath);
                product.Image = "~/Content/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult UpdateOrderStatus(int orderId, string status)
        {
            var order = db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Orders");
        }

        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Order Invoice Download (PDF)
        public ActionResult DownloadInvoice(int id)
        {
           
            var order = db.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null) return HttpNotFound();

            return new ViewAsPdf("InvoiceView", order)
            {
                FileName = "Invoice_Order_" + id + ".pdf"
            };
        }
    }
}