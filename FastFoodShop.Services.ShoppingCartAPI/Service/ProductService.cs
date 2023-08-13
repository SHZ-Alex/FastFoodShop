using FastFoodShop.Services.ShoppingCartAPI.Models.Dto;
using FastFoodShop.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace FastFoodShop.Services.ShoppingCartAPI.Service;

public class ProductService : IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory clientFactory)
    {
        _httpClientFactory = clientFactory;
    }
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        HttpClient client = _httpClientFactory.CreateClient("Product");
        HttpResponseMessage response = await client.GetAsync("/api/product");
        
        string apiContet = await response.Content.ReadAsStringAsync();
        
        ResponseDto resp = JsonConvert.DeserializeObject<ResponseDto>(apiContet);
        
        if (resp.IsSuccess)
            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
        
        return new List<ProductDto>();
    }
}