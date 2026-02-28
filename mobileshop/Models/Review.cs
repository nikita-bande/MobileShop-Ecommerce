using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mobileshop.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
        public System.DateTime ReviewDate { get; set; }
    }
}