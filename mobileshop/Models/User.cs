using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mobileshop.Models
{
    public class User
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "Nikita, please enter your name")] // Empty nahi reh sakta
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Sahi email format use karein")] // Email validation
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match.")] 
        public string ConfirmPassword { get; set; }

        
        
       [Required(ErrorMessage = "Mobile number is required")]
       [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string Mobile { get; set; }

        public string Role { get; set; }
    }
}