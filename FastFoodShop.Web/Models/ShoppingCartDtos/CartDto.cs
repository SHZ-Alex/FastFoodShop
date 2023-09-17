namespace FastFoodShop.Web.Models.ShoppingCartDtos;

public class CartDto
{
    public CartHeaderDto CartHeader { get; set; }
    public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
}