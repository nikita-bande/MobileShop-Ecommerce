namespace mobileshop.Migrations
{
    using mobileshop.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<mobileshop.Models.MobileShopDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(mobileshop.Models.MobileShopDBContext context)
        {
            context.Products.AddOrUpdate(p => p.ProductName,
     new Product { ProductName = "IPhone15", Price = 79999, IsActive = true, Category = "Mobile", Image = "/Content/Images/Iphone15.jpg" },
     new Product { ProductName = "Samsung s24", Price = 85000, IsActive = true, Category = "Mobile", Image = "/Content/Images/Samsung s24.jpg" },
     new Product { ProductName = "Charger", Price = 1500, IsActive = true, Category = "Accessory", Image = "/Content/Images/Charger.jpg" },
     new Product { ProductName = "Google Pixel 8", Price = 65000, IsActive = true, Category = "Mobile", Image = "/Content/Images/pixel8.jpg" },
     new Product { ProductName = "OnePlus 12", Price = 64999, IsActive = true, Category = "Mobile", Image = "/Content/Images/oneplus12.jpg" },
     new Product { ProductName = "AirPods Pro", Price = 24900, IsActive = true, Category = "Accessory", Image = "/Content/Images/airpods.jpg" },
     new Product { ProductName = "Leather Case", Price = 1200, IsActive = true, Category = "Accessory", Image = "/Content/Images/case.jpg" },
     new Product { ProductName = "Powerbank", Price = 499, IsActive = true, Category = "Accessory", Image = "/Content/Images/Powerbank.jpg" }
 );
        }
    }

}
