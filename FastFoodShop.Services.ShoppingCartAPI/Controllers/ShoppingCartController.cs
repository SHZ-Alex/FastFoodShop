using AutoMapper;
using FastFoodShop.Services.ShoppingCartAPI.Data;
using FastFoodShop.Services.ShoppingCartAPI.Models;
using FastFoodShop.Services.ShoppingCartAPI.Models.Dto;
using FastFoodShop.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.ShoppingCartAPI.Controllers;

[Route("api/shopping-cart")]
[ApiController]
public class ShoppingCartController : ControllerBase
{
    private ResponseDto _response;
    private readonly IMapper _mapper;
    private readonly AppDbContext _db;
    private readonly IProductService _productService;
    private readonly ICouponService _couponService;

    public ShoppingCartController(AppDbContext db,
        IMapper mapper,
        IProductService productService,
        ICouponService couponService)
    {
        _db = db;
        _response = new ResponseDto();
        _mapper = mapper;
        _productService = productService;
        _couponService = couponService;
    }
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(string userId)
    {
        try
        {
            CartDto cart = new()
            {
                CartHeader = _mapper.Map<CartHeaderDto>(await _db.CartHeaders.FirstAsync(x => x.UserId == userId))
            };
            
            cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(await _db.CartDetails
                .Where(x=>x.CartHeaderId == cart.CartHeader.Id)
                .ToArrayAsync());

            IEnumerable<ProductDto> productDtos = await _productService.GetProducts();

            foreach (var detail in cart.CartDetails)
            {
                detail.Product = productDtos.FirstOrDefault(u => u.ProductId == detail.ProductId);
                cart.CartHeader.CartTotal += detail.Count * detail.Product.Price;
            }

            if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
            {
                CouponDto coupon = await _couponService.GetCouponAsync(cart.CartHeader.CouponCode);
                
                if(coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                {
                    cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                    cart.CartHeader.Discount=coupon.DiscountAmount;
                }
            }

            _response.Result=cart;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.Message;
        }
        
        return Ok(_response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CartDto request)
    {
        try
        {
            CartHeader? cartHeaderFromDb = await _db.CartHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == request.CartHeader.UserId);
            
            if (cartHeaderFromDb is null)
            {
                //создаем детали карзины
                CartHeader cartHeader = _mapper.Map<CartHeader>(request.CartHeader);
                
                _db.CartHeaders.Add(cartHeader);
                await _db.SaveChangesAsync();
                
                request.CartDetails.First().CartHeaderId = cartHeader.Id;
                _db.CartDetails.Add(_mapper.Map<CartDetails>(request.CartDetails.First()));
                await _db.SaveChangesAsync();
            }
            else
            {
                // хедер есть, проверяем детали корзины
                CartDetails cartDetailsFromDb = await _db.CartDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProductId == request.CartDetails.First().ProductId 
                                              && x.CartHeaderId == cartHeaderFromDb.Id);
                if (cartDetailsFromDb is null)
                {
                    // создаем детали корзины
                    request.CartDetails.First().CartHeaderId = cartHeaderFromDb.Id;
                    _db.CartDetails.Add(_mapper.Map<CartDetails>(request.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
                else
                {
                    // обновляем количество в деталях
                    request.CartDetails.First().Count += cartDetailsFromDb.Count;
                    request.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                    request.CartDetails.First().CartDetailsId = cartDetailsFromDb.Id;
                    _db.CartDetails.Update(_mapper.Map<CartDetails>(request.CartDetails.First()));
                    await _db.SaveChangesAsync();
                }
            }
            _response.Result = request;
        }
        catch (Exception ex)
        {
            _response.Message= ex.Message;
            _response.IsSuccess= false;
            return BadRequest(_response);
        }
        return Ok(_response);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> RemoveCart(int id)
    {
        try
        {
            CartDetails cartDetails = await _db.CartDetails
                .SingleAsync(u => u.Id == id);

            int totalCountofCartItem = _db.CartDetails
                .Count(u => u.CartHeaderId == cartDetails.CartHeaderId);

            _db.CartDetails.Remove(cartDetails);
            
            if (totalCountofCartItem == 1)
            {
                CartHeader cartHeaderToRemove = await _db.CartHeaders
                    .SingleAsync(u => u.Id == cartDetails.CartHeaderId);

                _db.CartHeaders.Remove(cartHeaderToRemove);
            }
            await _db.SaveChangesAsync();

            _response.Result = true;
        }
        catch (Exception ex)
        {
            _response.Message = ex.Message.ToString();
            _response.IsSuccess = false;
        }
        return Ok(_response);
    }
    
    [HttpPost("apply-coupon")]
    public async Task<IActionResult> ApplyCoupon([FromBody] CartDto request)
    {
        try
        {
            CartHeader cartFromDb = await _db.CartHeaders
                .FirstAsync(x => x.UserId == request.CartHeader.UserId);
            
            cartFromDb.CouponCode = request.CartHeader.CouponCode;
            
            _db.CartHeaders.Update(cartFromDb);
            await _db.SaveChangesAsync();
            
            _response.Result = true;
        }
        catch (Exception ex)
        {
            _response.IsSuccess = false;
            _response.Message = ex.ToString();
        }
        return Ok(_response);
    }
}