using mobileshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mobileshop.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        MobileShopDBContext db = new MobileShopDBContext();
        public ActionResult Index()
        {
            var products = db.Products.ToList(); // all products will show
            return View(products);
        }
    }
}