using AutoMapper;
using FastFoodShop.Services.CouponAPI.Data;
using FastFoodShop.Services.CouponAPI.Models;
using FastFoodShop.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.CouponAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CouponController : ControllerBase
{
    private readonly AppDbContext _db;
    private ResponseDto _response;
    private readonly IMapper _mapper;

    public CouponController(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _response = new ResponseDto();
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            IEnumerable<Coupon> obj = await _db.Coupons.ToListAsync();
            _response.Result = _mapper.Map<IEnumerable<CouponDto>>(obj);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Ok(_response);
    }

    [HttpGet]
    [Route("{couponId:int}")]
    public async Task<IActionResult> Get(int couponId)
    {
        try
        {
            Coupon obj = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == couponId);
            _response.Result =_mapper.Map<CouponDto>(obj);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Ok(_response);
    }
    
    [HttpGet]
    [Route("{code}")]
    public async Task<IActionResult> Get(string couponCode)
    {
        try
        {
            Coupon coupon = await _db.Coupons.FirstOrDefaultAsync(x => x.CouponCode.ToLower() == couponCode.ToLower());
            _response.Result =_mapper.Map<CouponDto>(coupon);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
        }

        return Ok(_response);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CouponDto request)
    {
        try
        {
            Coupon obj = _mapper.Map<Coupon>(request);
            _db.Coupons.Add(obj);
            await _db.SaveChangesAsync();
            _response.Result =_mapper.Map<CouponDto>(obj);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
        }

        return Ok(_response);
    }
    
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CouponDto request)
    {
        try
        {
            Coupon obj = _mapper.Map<Coupon>(request);
            _db.Coupons.Update(obj);
            await _db.SaveChangesAsync();
            _response.Result =_mapper.Map<CouponDto>(obj);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
        }

        return Ok(_response);
    }
    
    [HttpDelete]
    [Route("{couponId:int}")]
    public async Task<IActionResult> Delete(int couponId)
    {
        try
        {
            Coupon obj = await _db.Coupons.FirstAsync(x => x.Id == couponId);
            _db.Coupons.Remove(obj);
            await _db.SaveChangesAsync();
            _response.Result =_mapper.Map<CouponDto>(obj);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
        }

        return Ok(_response);
    }
    
}