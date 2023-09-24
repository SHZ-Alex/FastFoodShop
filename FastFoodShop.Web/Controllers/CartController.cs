using System.IdentityModel.Tokens.Jwt;
using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Models.OrderDtos;
using FastFoodShop.Web.Models.ShoppingCartDtos;
using FastFoodShop.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
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

        if (response is not null && response.IsSuccess)
        {
            string domain = Request.Scheme + "://" + Request.Host.Value + "/";

            StripeRequestDto stripeRequestDto = new()
            {
                ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                CancelUrl = domain + "cart/checkout",
                OrderHeader = orderHeaderDto
            };

            ResponseDto stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
            
            StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
                (Convert.ToString(stripeResponse.Result));
            
            Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
            return new StatusCodeResult(303);
        }
        
        return View();
    }
    
    public async Task<IActionResult> Confirmation(int orderId)
    {
        ResponseDto? response = await _orderService.ValidateStripeSession(orderId);
        
        if (response != null & response.IsSuccess)
        {

            OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            
            if (orderHeader.Status == Status.Approved.GetDisplayName())
            {
                return View(orderId);
            }
        } 
        
        return View(orderId);
    }
    
    [HttpPost]
    public async Task<IActionResult> EmailCart(CartDto cartDto)
    {
        CartDto cart = await LoadCartDtoBasedOnLoggedInUser();
        cart.CartHeader.Email = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value!;
        
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
            .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
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
        string userId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value!;
        
        ResponseDto? response = await _cartService.GetCartByUserIdAsnyc(userId);

        if (response == null || !response.IsSuccess) return new CartDto();
        
        CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result)!)!;
        return cartDto;
    }
}