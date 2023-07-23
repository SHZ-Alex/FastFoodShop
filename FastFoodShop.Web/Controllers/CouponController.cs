using FastFoodShop.Web.Models;
using FastFoodShop.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FastFoodShop.Web.Controllers;

public class CouponController : Controller
{
    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }
    public async Task<IActionResult> CouponIndex()
    {
        List<CouponDto> counpons = new List<CouponDto>();

        ResponseDto responseDto = await _couponService.GetAllCouponAsync();

        if (responseDto.IsSuccess)
            counpons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
        else
            TempData["error"] = responseDto?.Message;
        
        
        return View(counpons);
    }
    
    public async Task<IActionResult> CouponCreate()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CouponCreate(CouponDto request)
    {
        if (!ModelState.IsValid)
            return View(request);
        
        ResponseDto responseDto = await _couponService.CreateCouponAsync(request);

        if (responseDto.IsSuccess)
        {
            TempData["success"] = "Купон успешно создан";
            return RedirectToAction(nameof(CouponIndex));
        }

        TempData["error"] = responseDto?.Message;
        return View();
    }
    
    public async Task<IActionResult> CouponDelete(int couponId)
    {
        ResponseDto responseDto = await _couponService.GetCouponByIdAsync(couponId);

        if (responseDto.IsSuccess)
        {
            CouponDto? couponDto = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            return View(couponDto);
        }
        
        TempData["error"] = responseDto?.Message;
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> CouponDelete(CouponDto request)
    {
        ResponseDto responseDto = await _couponService.DeleteCouponAsync(request.Id);

        if (responseDto.IsSuccess)
        {
            TempData["success"] = "Купон успешно удален";
            return RedirectToAction(nameof(CouponIndex));
        }

        TempData["error"] = responseDto?.Message;
        return View(request);
    }
    
}