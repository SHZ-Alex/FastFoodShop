using FastFood.Services.ProductAPI.Models.Dto;

namespace FastFood.Services.ProductAPI.Handlers.IHandlers;

public interface IProductHandler
{
    Task<ProductHandlerGetFileNameResultDto> GetFileName(ProductHandlerGetFileNameDto getFileNameDto, HttpContext context);
    void DeleteImage(string path);
}