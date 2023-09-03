using System.ComponentModel.DataAnnotations;
using FastFoodShop.Services.OrderAPI.Models.Enums;

namespace FastFoodShop.Services.OrderAPI.Models;

public class OrderHeader
{
    [Key]
    public int OrderHeaderId { get; set; }
    public string? UserId { get; set; }
    public string? CouponCode { get; set; }
    public double Discount { get; set; }
    public double OrderTotal { get; set; }


    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }
    public DateTime OrderTime { get; set; }
    public Status? Status { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? StripeSessionId { get; set; }
    public ICollection<OrderDetails>? OrderDetails { get; set; }
}