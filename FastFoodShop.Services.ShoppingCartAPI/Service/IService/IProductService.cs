using FastFoodShop.Services.ShoppingCartAPI.Models.Dto;

namespace FastFoodShop.Services.ShoppingCartAPI.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts();
}