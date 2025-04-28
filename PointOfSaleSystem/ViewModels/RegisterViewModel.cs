using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller: "Account", ErrorMessage = "This email is already registered")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
