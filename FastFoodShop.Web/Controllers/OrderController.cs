using System.IdentityModel.Tokens.Jwt;
using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Models.OrderDtos;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using Newtonsoft.Json;

namespace FastFoodShop.Web.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    public IActionResult OrderIndex()
    {
        return View();
    }
    
    [HttpPost("OrderReadyForPickup")]
    public async Task<IActionResult> OrderReadyForPickup(int orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId, (int)Status.ReadyForPickup);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }
        // ReSharper disable once Mvc.ViewNotResolved
        return View();
    }

    [HttpPost("CompleteOrder")]
    public async Task<IActionResult> CompleteOrder(int orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId, (int)Status.Completed);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }
        // ReSharper disable once Mvc.ViewNotResolved
        return View();
    }

    [HttpPost("CancelOrder")]
    public async Task<IActionResult> CancelOrder(int orderId)
    {
        var response = await _orderService.UpdateOrderStatus(orderId, (int)Status.Cancelled);
        if (response != null && response.IsSuccess)
        {
            TempData["success"] = "Status updated successfully";
            return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });
        }
        // ReSharper disable once Mvc.ViewNotResolved
        return View();
    }
    
    public async Task<IActionResult> OrderDetail(int orderId)
    {
        OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
        string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

        var response = await _orderService.GetOrder(orderId);
        if (response != null && response.IsSuccess)
        {
            orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
        }
        if(!User.IsInRole(SD.RoleAdmin) && userId!= orderHeaderDto.UserId)
        {
            return NotFound();
        }
        return View(orderHeaderDto);
    }
    
    [HttpGet]
    public IActionResult GetAll() 
    {
        IEnumerable<OrderHeaderDto> list;
        string userId = "";
        if (!User.IsInRole(SD.RoleAdmin))
        {
            userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
        }
        ResponseDto response = _orderService.GetAllOrder(userId).GetAwaiter().GetResult();
        if (response != null && response.IsSuccess)
        {
            list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
        }
        else
        {
            list = new List<OrderHeaderDto>();
        }
        return Json(new { data = list });
    }
}