using FastFoodShop.Web.Models;

namespace FastFoodShop.Web.Services.IServices;

public interface IBaseService
{
    Task<ResponseDto> SendAsync(RequestDto request, bool withBearer = true);
}