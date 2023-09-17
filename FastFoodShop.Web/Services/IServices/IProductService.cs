using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.ProductDtos;

namespace FastFoodShop.Web.Services.IServices;

public interface IProductService
{
    Task<ResponseDto?> GetProductAsync(string couponCode);
    Task<ResponseDto?> GetAllProductsAsync();
    Task<ResponseDto?> GetProductByIdAsync(int id);
    Task<ResponseDto?> CreateProductsAsync(ProductDto productDto);
    Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto);
    Task<ResponseDto?> DeleteProductsAsync(int id);
}