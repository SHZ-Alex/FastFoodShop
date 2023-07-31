using AutoMapper;
using FastFoodShop.Services.CouponAPI.Data;
using FastFoodShop.Services.CouponAPI.Models;
using FastFoodShop.Services.CouponAPI.Models.Dto;
using FastFoodShop.Services.CouponAPI.Repository.IRepository;
using FastFoodShop.Services.CouponAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.CouponAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CouponController : ControllerBase
{
    private ResponseDto _response;
    private readonly IMapper _mapper;
    private readonly ICouponRepository _repository;

    public CouponController(IMapper mapper,
        ICouponRepository repository)
    {
        _response = new ResponseDto();
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            IEnumerable<Coupon> coupons = await _repository.GetAllAsync();
            _response.Result = _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
            return NotFound(_response);
        }

        return Ok(_response);
    }

    [HttpGet("{couponId:int}")]
    public async Task<IActionResult> Get(int couponId)
    {
        try
        {
            Coupon coupon = await _repository.GetAsync(x => x.Id == couponId);
            _response.Result =_mapper.Map<CouponDto>(coupon);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
            return NotFound(_response);
        }

        return Ok(_response);
    }
    
    [HttpGet("{couponCode}")]
    public async Task<IActionResult> Get(string couponCode)
    {
        try
        {
            Coupon coupon = await _repository.GetAsync(x => x.CouponCode == couponCode);
            _response.Result =_mapper.Map<CouponDto>(coupon);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
            return NotFound(_response);
        }

        return Ok(_response);
    }
    
    [HttpPost]
    [Authorize(Roles = SD.RoleAdmin)]
    public async Task<IActionResult> Post([FromBody] CouponDto request)
    {
        try
        {
            Coupon coupon = _mapper.Map<Coupon>(request);
            await _repository.CreateAsync(coupon);
            _response.Result =_mapper.Map<CouponDto>(coupon);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
            return BadRequest(_response);
        }

        return Ok(_response);
    }
    
    [HttpPut]
    [Authorize(Roles = SD.RoleAdmin)]
    public async Task<IActionResult> Put([FromBody] CouponDto request)
    {
        try
        {
            Coupon coupon = _mapper.Map<Coupon>(request);
            await _repository.UpdateAsync(coupon);
            _response.Result =_mapper.Map<CouponDto>(coupon);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
            return BadRequest(_response);
        }

        return Ok(_response);
    }
    
    [HttpDelete("{couponId:int}")]
    [Authorize(Roles = SD.RoleAdmin)]
    public async Task<IActionResult> Delete(int couponId)
    {
        try
        {
            Coupon coupon = await _repository.GetAsync(x => x.Id == couponId);
            await _repository.RemoveAsync(coupon);
            _response.Result =_mapper.Map<CouponDto>(coupon);
        }
        catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.Message = e.Message;
            return NotFound(_response);
        }

        return Ok(_response);
    }
    
}