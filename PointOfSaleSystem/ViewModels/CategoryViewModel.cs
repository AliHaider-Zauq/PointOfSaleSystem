using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [Remote(action: "IsCategoryNameAvailable", controller: "Category",ErrorMessage = "Category name is already in use")]
        public string Name { get; set; } = string.Empty;
    }

}
