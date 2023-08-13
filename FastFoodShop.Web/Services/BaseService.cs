using System.Net;
using System.Text;
using FastFoodShop.Web.Models;
using FastFoodShop.Web.Models.Enums;
using FastFoodShop.Web.Services.IServices;
using FastFoodShop.Web.Utility;
using Newtonsoft.Json;

namespace FastFoodShop.Web.Services;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenProvider _tokenProvider;
    
    public BaseService(IHttpClientFactory httpClientFactory,
        ITokenProvider tokenProvider)
    {
        _httpClientFactory = httpClientFactory;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<ResponseDto> SendAsync(RequestDto request, bool withBearer = true)
    {
        try
        {
            HttpClient cliend = _httpClientFactory.CreateClient("FastFoodShopAPI");
            HttpRequestMessage message = new HttpRequestMessage();
        
            message.Headers.Add("Accept", "application/json");
            
            if (withBearer)
            {
                string token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }

            message.RequestUri = new Uri(request.Url);

            if (request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }

            switch (request.ApiType)
            {
                case ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }

            HttpResponseMessage apiResponse = await cliend.SendAsync(message);

            switch (apiResponse.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new ResponseDto { IsSuccess = false , Message = "Not found"};
                case HttpStatusCode.Forbidden:
                    return new ResponseDto { IsSuccess = false , Message = "Forbidden"};
                case HttpStatusCode.Unauthorized:
                    return new ResponseDto { IsSuccess = false , Message = "Unauthorized"};
                case HttpStatusCode.InternalServerError:
                    return new ResponseDto { IsSuccess = false , Message = "Internal Server Error"};
                case HttpStatusCode.BadRequest:
                    return new ResponseDto { IsSuccess = false, Message = "Bad request" };
                default:
                    string apiContent = await apiResponse.Content.ReadAsStringAsync();
                    ResponseDto? apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    return apiResponseDto;
            }
        }
        catch (Exception e)
        {
            ResponseDto dto = new ResponseDto
            {
                Message = e.Message,
                IsSuccess = false
            };
            return dto;
        }
    }
}