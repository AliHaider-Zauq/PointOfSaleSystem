﻿namespace PointOfSaleSystem.Models;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; }

    public decimal SalePrice { get; set; }         
    public int Quantity { get; set; }               

    public int CategoryId { get; set; }
    public int SupplierId { get; set; }

    public Category Category { get; set; }
    public Supplier Supplier { get; set; }
}
