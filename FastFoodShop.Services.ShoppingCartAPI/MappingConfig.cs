using AutoMapper;
using FastFoodShop.Services.ShoppingCartAPI.Models;
using FastFoodShop.Services.ShoppingCartAPI.Models.Dto;

namespace FastFoodShop.Services.ShoppingCartAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
            config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
        });
        return mappingConfig;
    }
}