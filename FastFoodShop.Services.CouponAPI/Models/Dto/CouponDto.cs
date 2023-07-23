using System;
namespace FastFoodShop.Services.CouponAPI.Models.Dto;

public class CouponDto
{
    public int Id { get; set; }
    public string CouponCode { get; set; }
    public string DiscountAmount { get; set; }
    public int MinAmount { get; set; }
}

