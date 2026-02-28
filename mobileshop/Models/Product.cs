using System.ComponentModel.DataAnnotations;

namespace mobileshop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string ProductName { get; set; }

        public string Description { get; set; }

        [Range(1, 100000)]
        public decimal Price { get; set; }
        public string Image { get; set; }
      
        public bool IsActive { get; set; } = true;

        public string Category { get; set; } 
        public int Rating { get; set; }
        public int Quantity { get; set; } 
    }
}
  