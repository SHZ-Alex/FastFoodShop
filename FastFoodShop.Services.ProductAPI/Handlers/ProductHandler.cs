using FastFood.Services.ProductAPI.Handlers.IHandlers;
using FastFood.Services.ProductAPI.Models.Dto;

namespace FastFood.Services.ProductAPI.Handlers;

public class ProductHandler : IProductHandler
{
    public async Task<ProductHandlerGetFileNameResultDto> GetFileName(ProductHandlerGetFileNameDto getFileNameDto, HttpContext context)
    {
        
        string fileName = Path.GetRandomFileName() + Path.GetExtension(getFileNameDto.Image.FileName);
        string filePath = "wwwroot/ProductImages/" + fileName;
        string filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
        
        await using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
        {
            await getFileNameDto.Image.CopyToAsync(fileStream);
        }
        
        string baseUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}{context.Request.PathBase.Value}";
        string urlName = baseUrl + "/ProductImages/" + fileName;
        
        return new ProductHandlerGetFileNameResultDto(urlName, filePath);
    }
    
    public void DeleteImage(string imageLocalPath)
    {
        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), imageLocalPath);
        FileInfo file = new FileInfo(oldFilePathDirectory);
        if (file.Exists)
            file.Delete();
    }
}