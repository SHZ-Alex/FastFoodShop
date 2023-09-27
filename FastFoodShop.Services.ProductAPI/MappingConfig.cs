using AutoMapper;
using FastFood.Services.ProductAPI.Models;
using FastFood.Services.ProductAPI.Models.Dto;

namespace FastFood.Services.ProductAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductDto, Product>().ReverseMap();
            config.CreateMap<ProductDto, ProductHandlerGetFileNameDto>();
            config.CreateMap<Product, ProductHandlerGetFileNameDto>();
        });
        return mappingConfig;
    }
}