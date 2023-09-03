using System.IdentityModel.Tokens.Jwt;
using FastFoodShop.Web.Models;
using FastFoodShop.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFoodShop.Web.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    public CartController(ICartService cartService, IOrderService orderService)
    {
        _cartService = cartService;
        _orderService = orderService;
    }

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await LoadCartDtoBasedOnLoggedInUser());
    }
    
    [Authorize]
    public async Task<IActionResult> Checkout()
    {
        return View(await LoadCartDtoBasedOnLoggedInUser());
    }
    
    [HttpPost]
    public async Task<IActionResult> Checkout(CartDto request)
    {

        CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
        cart.CartHeader.Phone = request.CartHeader.Phone;
        cart.CartHeader.Email = request.CartHeader.Email;
        cart.CartHeader.Name = request.CartHeader.Name;

        var response = await _orderService.CreateOrder(cart);
        OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

        if (response != null && response.IsSuccess)
        {
             
        }
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> EmailCart(CartDto cartDto)
    {
        CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
        cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;
        
        ResponseDto? response = await _cartService.EmailCart(cart);
        
        if (response != null & response.IsSuccess)
        {
            TempData["success"] = "Электронное письмо будет обработано и отправлено в ближайшее время.";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }
    
    public async Task<IActionResult> Remove(int cartDetailsId)
    {
        var userId = User.Claims
            .Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        
        ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);
        if (response != null & response.IsSuccess)
        {
            TempData["success"] = "Корзина успешно обновлена";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ApplyCoupon(CartDto request)
    {

        ResponseDto? response = await _cartService.ApplyCouponAsync(request);
        if (response != null & response.IsSuccess)
        {
            TempData["success"] = "Корзина успешно обновлена";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> RemoveCoupon(CartDto request)
    {
        request.CartHeader.CouponCode = "";
        ResponseDto? response = await _cartService.ApplyCouponAsync(request);
        if (response != null & response.IsSuccess)
        {
            TempData["success"] = "Корзина успешно обновлена";
            return RedirectToAction(nameof(CartIndex));
        }
        return View();
    }

    private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
    {
        string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        
        ResponseDto? response = await _cartService.GetCartByUserIdAsnyc(userId);

        if (response == null || !response.IsSuccess) return new CartDto();
        
        CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
        return cartDto;
    }
}