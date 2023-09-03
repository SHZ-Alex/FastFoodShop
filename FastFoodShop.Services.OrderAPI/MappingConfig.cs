using AutoMapper;
using FastFoodShop.Services.OrderAPI.Models;
using FastFoodShop.Services.OrderAPI.Models.Dto;

namespace FastFoodShop.Services.OrderAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(x=> x.CartTotal, 
                    y=> y.MapFrom(z=>z.OrderTotal)).ReverseMap();
            config.CreateMap<CartDetailsDto, OrderDetailsDto>()
                .ForMember(x => x.ProductName,
                    y => y.MapFrom(z => z.Product.Name))
                .ForMember(x => x.Price, 
                    y => y.MapFrom(z => z.Product.Price));
            config.CreateMap<OrderDetailsDto, CartDetailsDto>();
            config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
            config.CreateMap<OrderDetailsDto, OrderDetails>().ReverseMap();
               
        });
        return mappingConfig;
    }
}