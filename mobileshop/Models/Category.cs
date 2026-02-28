using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobileshop.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
    }
}