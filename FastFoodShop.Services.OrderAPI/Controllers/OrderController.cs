using AutoMapper;
using FastFoodShop.MessageBus;
using FastFoodShop.Services.OrderAPI.Data;
using FastFoodShop.Services.OrderAPI.Models;
using FastFoodShop.Services.OrderAPI.Models.Dto;
using FastFoodShop.Services.OrderAPI.Models.Enums;
using FastFoodShop.Services.OrderAPI.Service.IService;
using FastFoodShop.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Stripe;
using Stripe.Checkout;

namespace FastFoodShop.Services.OrderAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private ResponseDto _response;
    private readonly IMapper _mapper;
    private readonly AppDbContext _db;
    private readonly IProductService _productService;
    
    public OrderController(AppDbContext db, IProductService productService, IMapper mapper)
    {
        _db = db;
        _response = new ResponseDto();
        _productService = productService;
        _mapper = mapper;
    }

    [Authorize]
    [HttpPost]
    public async Task<ResponseDto> Post([FromBody] CartDto request)
    {
        try
        {
            OrderHeaderDto orderHeaderDto = _mapper.Map<OrderHeaderDto>(request.CartHeader);
            orderHeaderDto.OrderTime = DateTime.Now;
            orderHeaderDto.Status = Status.Pending.GetDisplayName();
            orderHeaderDto.OrderDetails = _mapper.Map<IEnumerable<OrderDetailsDto>>(request.CartDetails);

            OrderHeader orderCreated =  _db.OrderHeaders.Add(_mapper.Map<OrderHeader>(orderHeaderDto)).Entity;
            await _db.SaveChangesAsync();

            orderHeaderDto.OrderHeaderId = orderCreated.OrderHeaderId;
            _response.Result = orderHeaderDto;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message=ex.Message;
        }
        return _response;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get(string? userId)
    {
        try
        {
            IEnumerable<OrderHeader> response;
            if (User.IsInRole(SD.RoleAdmin))
            {
                response = await _db.OrderHeaders
                    .Include(x => x.OrderDetails)
                    .OrderByDescending(x => x.OrderHeaderId)
                    .ToArrayAsync();
            }
            else
            {
                response = await _db.OrderHeaders
                    .Include(x => x.OrderDetails)
                    .Where(x=> x.UserId == userId)
                    .OrderByDescending(x => x.OrderHeaderId)
                    .ToArrayAsync();
            }
            _response.Result = _mapper.Map<IEnumerable<OrderHeaderDto>>(response);
        }
        catch (Exception ex) 
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return BadRequest(_response);
        }
        return Ok(_response);
    }
    
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            OrderHeader orderHeader = await _db.OrderHeaders
                .Include(u => u.OrderDetails)
                .FirstAsync(u => u.OrderHeaderId == id);
            
            _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        return Ok(_response);
    }
    
    [Authorize]
    [HttpPut("{orderId:int}/{newStatus:int}")]
    public async Task<IActionResult> Put(int orderId, int newStatus)
    {
        try
        {
            OrderHeader? orderHeader = await _db.OrderHeaders
                .FirstOrDefaultAsync(x => x.OrderHeaderId == orderId);
            
            if (orderHeader != null)
            {
                if(newStatus == (int)Status.Cancelled)
                {
                    var options = new RefundCreateOptions
                    {
                        Reason = RefundReasons.RequestedByCustomer,
                        PaymentIntent = orderHeader.PaymentIntentId
                    };

                    RefundService service = new RefundService();
                    await service.CreateAsync(options);
                }
                Status status = (Status)newStatus;
                orderHeader.Status = status.GetDisplayName();
                await _db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
            return BadRequest(_response);
        }
        return Ok(_response);
    }
}