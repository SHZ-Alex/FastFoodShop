namespace FastFood.Services.ProductAPI.Models.Dto;

public class ProductHandlerGetFileNameResultDto
{
    public ProductHandlerGetFileNameResultDto(string urlName, string filePath)
    {
        UrlName = urlName;
        FilePath = filePath;
    }
    public string UrlName { get; set; }
    public string FilePath { get; set; }
}