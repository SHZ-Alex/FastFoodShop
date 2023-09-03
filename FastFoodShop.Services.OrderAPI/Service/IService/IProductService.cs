using FastFoodShop.Services.OrderAPI.Models.Dto;

namespace FastFoodShop.Services.OrderAPI.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts();
}