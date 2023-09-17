using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.ProductDtos;
using FastFoodShop.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFoodShop.Web.Controllers;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    
    public ProductController(IProductService productService) 
    { 
        _productService=productService;
    }

    public async Task<IActionResult> ProductIndex()
    {
        List<ProductDto>? list = new();

        ResponseDto? response = await _productService.GetAllProductsAsync();

        if (response != null && response.IsSuccess)
            list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        else
            TempData["error"] = response?.Message;

        return View(list);
    }

    public async Task<IActionResult> ProductCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductDto model)
    {
        if (ModelState.IsValid)
        {
            ResponseDto? response = await _productService.CreateProductsAsync(model);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Продукт был добавлен";
                return RedirectToAction(nameof(ProductIndex));
            }
            
            TempData["error"] = response?.Message;
        }

        return View(model);
    }

    public async Task<IActionResult> ProductDelete(int productId)
    {
        ResponseDto? response = await _productService.GetProductByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
            ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(model);
        }
        
        TempData["error"] = response?.Message;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductDelete(ProductDto request)
    {
        ResponseDto? response = await _productService.DeleteProductsAsync(request.ProductId);

        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Продукт был удален";
            return RedirectToAction(nameof(ProductIndex));
        }

        TempData["error"] = response?.Message;
        return View(request);
    }
    
    public async Task<IActionResult> ProductEdit(int productId)
    {
        ResponseDto? response = await _productService.GetProductByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
            ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            return View(model);
        }
        
        TempData["error"] = response?.Message;
        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductDto request)
    {
        ResponseDto? response = await _productService.UpdateProductsAsync(request);

        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Продукт был обновлен";
            return RedirectToAction(nameof(ProductIndex));
        }
        
        TempData["error"] = response?.Message;
        return View(request);
    }
}