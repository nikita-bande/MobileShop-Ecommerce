using mobileshop.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace mobileshop.Controllers
{
    public class AccountController : Controller
    {
        //for connect to th database
        MobileShopDBContext db = new MobileShopDBContext();

        // 1. Registration Page (GET)
        public ActionResult Register()
        {
            return View();
        }

        // 2. Registration Logic (POST)
        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Here we have hardcoded the role
                // Now no one will be able to make themselves an admin even if they want to
                user.Role = "Customer";

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }
        // 3. Login Page (GET)
        public ActionResult Login()
        {
            return View();
        }

        // 4. Login Logic (POST)
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                Session["UserId"] = user.Id;
                Session["UserName"] = user.Name;
                Session["UserRole"] = user.Role; // Role save karein

                // redirect by role
                if (user.Role == "Admin")
                {
                    Session["UserRole"] = "Admin";
                    Session["UserName"] = user.Name;
                    // previously this could be RedirectToAction("Index", "Admin") or just "Admin" 
                    // change this to Dashboard:
                    return RedirectToAction("Dashboard", "Admin");
                }
            }
            ViewBag.Error = "Invalid email or password. Please try again.";
            return View();
        }
        public ActionResult MyOrders()
        {
          
            if (Session["UserId"] != null)
            {
                int loggedInUserId = Convert.ToInt32(Session["UserId"]);

           
                var orders = db.Orders.Where(o => o.UserId == loggedInUserId)
                                      .OrderByDescending(o => o.OrderDate).ToList();

                return View(orders);
            }
            return RedirectToAction("Login");
        }

        // 5. Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Product");
        }

        // 1. Forgot Password Page (GET)
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // 2. Forgot Password Logic (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string email, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }

            var user = db.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
         
                user.Password = newPassword;

                db.Users.Attach(user);
                db.Entry(user).Property(x => x.Password).IsModified = true;

                db.Configuration.ValidateOnSaveEnabled = false;

                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true; 

                TempData["Success"] = "Password reset successfully!";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Email not found.";
            return View();
        }
    }
}