using AutoMapper;
using FastFoodShop.MessageBus;
using FastFoodShop.Services.OrderAPI.Data;
using FastFoodShop.Services.OrderAPI.Models;
using FastFoodShop.Services.OrderAPI.Models.Dto;
using FastFoodShop.Services.OrderAPI.Models.Enums;
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
public class PaymentController : ControllerBase
{
    private readonly ResponseDto _response;
    private readonly IMapper _mapper;
    private readonly AppDbContext _db;
    private readonly IMessageBus _messageBus;
    
    public PaymentController(AppDbContext db, IMapper mapper, IMessageBus messageBus)
    {
        _db = db;
        _response = new ResponseDto();
        _mapper = mapper;
        _messageBus = messageBus;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PaymentPost([FromBody] StripeRequestDto request)
    {
        try
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = request.ApprovedUrl,
                CancelUrl = request.CancelUrl,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            var discount = new List<SessionDiscountOptions>()
            {
                new SessionDiscountOptions
                {
                    Coupon = request.OrderHeader.CouponCode
                }
            };

            foreach (OrderDetailsDto item in request.OrderHeader.OrderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Count
                };

                options.LineItems.Add(sessionLineItem);
            }

            if (request.OrderHeader.Discount > 0)
            {
                options.Discounts = discount;
            }

            SessionService service = new SessionService();
            Session session = await service.CreateAsync(options);
            request.StripeSessionUrl = session.Url;
            
            OrderHeader orderHeader = await _db.OrderHeaders
                .FirstAsync(u => u.OrderHeaderId == request.OrderHeader.OrderHeaderId);
            
            orderHeader.StripeSessionId = session.Id;
            await _db.SaveChangesAsync();
            _response.Result = request;
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }

        return Ok(_response);
    }
    
    [Authorize]
    [HttpGet("{orderHeaderId}")]
    public async Task<IActionResult> Get(int orderHeaderId)
    {
        try
        {
            OrderHeader orderHeader = await _db.OrderHeaders.FirstAsync(x => x.OrderHeaderId == orderHeaderId);

            SessionService service = new SessionService();
            Session session = await service.GetAsync(orderHeader.StripeSessionId);

            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent = await paymentIntentService.GetAsync(session.PaymentIntentId);

            if(paymentIntent.Status == "succeeded")
            {
                orderHeader.PaymentIntentId = paymentIntent.Id;
                orderHeader.Status = Status.Approved.GetDisplayName();
                await _db.SaveChangesAsync();
                
                RewardsDto rewardsDto = new()
                {
                    OrderId = orderHeader.OrderHeaderId,
                    RewardsActivity = Convert.ToInt32(orderHeader.OrderTotal),
                    UserId = orderHeader.UserId
                };
                
                await _messageBus.PublishMessage(rewardsDto, SD.OrderCreatedTopicName);
                
                _response.Result = _mapper.Map<OrderHeaderDto>(orderHeader);
            }

        }
        catch (Exception ex)
        {
            _response.Message = ex.Message;
            _response.IsSuccess = false;
            return BadRequest(_response);
        }
        return Ok(_response);
    }
}