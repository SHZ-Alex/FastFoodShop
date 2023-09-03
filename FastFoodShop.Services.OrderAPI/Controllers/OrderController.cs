using AutoMapper;
using FastFoodShop.Services.OrderAPI.Data;
using FastFoodShop.Services.OrderAPI.Models;
using FastFoodShop.Services.OrderAPI.Models.Dto;
using FastFoodShop.Services.OrderAPI.Models.Enums;
using FastFoodShop.Services.OrderAPI.Service.IService;
using FastFoodShop.Services.OrderAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace FastFoodShop.Services.OrderAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private ResponseDto _response;
    private readonly IMapper _mapper;
    private readonly AppDbContext _db;
    private readonly IProductService _productService;
    
    public OrderController(AppDbContext db,
        IProductService productService, 
        IMapper mapper)
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
}