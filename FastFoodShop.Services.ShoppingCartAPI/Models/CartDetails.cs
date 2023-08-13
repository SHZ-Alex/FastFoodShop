using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using FastFoodShop.Services.ShoppingCartAPI.Models.Dto;

namespace FastFoodShop.Services.ShoppingCartAPI.Models;

public class CartDetails
{
    [Key]
    public int Id { get; set; }

    public int CartHeaderId { get; set; }
    [JsonIgnore]
    [ForeignKey("CartHeaderId")]
    public CartHeader CartHeader { get; set; }

    public int ProductId { get; set; }
    [NotMapped]
    public ProductDto Product { get; set; }

    public int Count { get; set; }
}