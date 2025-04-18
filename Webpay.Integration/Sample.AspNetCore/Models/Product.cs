﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.AspNetCore.Models;

public class Product
{
    public string Class { get; set; }
    public string ImageUrl { get; set; }
    public string ItemUrl { get; set; }
    public string Name { get; set; }

    [Required(ErrorMessage = "Please provide a number greater than zero!")]
    [Display(Name = "Unit price")]
    [Range(1, int.MaxValue)]
    public decimal Price { get; set; }

    [Display(Name = "Discount Amount")]
    [Range(1, int.MaxValue)]
    public decimal DiscountAmount { get; set; }

    [Display(Name = "Discount Percent")]
    [Range(0, 100)]
    public decimal DiscountPercent { get; set; }

    public int ProductId { get; set; }
    public string Reference { get; set; }
    public string Type { get; set; }

    public int VatPercentage { get; set; }
}
