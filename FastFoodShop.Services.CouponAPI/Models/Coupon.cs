using System;
using System.ComponentModel.DataAnnotations;

namespace FastFoodShop.Services.CouponAPI.Models;

public class Coupon
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string CouponCode { get; set; }
    [Required]
    public double DiscountAmount { get; set; }
    public double MinAmount { get; set; }
}

