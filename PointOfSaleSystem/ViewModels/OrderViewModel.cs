using Microsoft.AspNetCore.Mvc.Rendering;
using PointOfSaleSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace PointOfSaleSystem.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; } = new();

        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Customer>? Customers { get; set; }
    }

}
