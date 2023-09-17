using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Models.ProductDtos;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;

namespace FastFoodShop.Web.Services;

public class ProductService : IProductService
{
    private const string Url = "/api/product/";
    private readonly IBaseService _baseService;
    public ProductService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto() 
        {
            ApiType = ApiType.POST,
            Data=productDto,
            Url = SD.ProductAPIBase + "/api/product" 
        });
    }

    public async Task<ResponseDto?> DeleteProductsAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.DELETE,
            Url = SD.ProductAPIBase + Url + id
        }); 
    }

    public async Task<ResponseDto?> GetAllProductsAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.ProductAPIBase + Url
        });
    }

    public async Task<ResponseDto?> GetProductAsync(string productCode)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.ProductAPIBase + Url + productCode
        });
    }

    public async Task<ResponseDto?> GetProductByIdAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.GET,
            Url = SD.ProductAPIBase + Url + id
        });
    }

    public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = ApiType.PUT,
            Data = productDto,
            Url = SD.ProductAPIBase + Url
        });
    }
}