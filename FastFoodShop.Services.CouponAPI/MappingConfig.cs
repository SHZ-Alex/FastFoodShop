using AutoMapper;
using FastFoodShop.Services.CouponAPI.Models;
using FastFoodShop.Services.CouponAPI.Models.Dto;

namespace FastFoodShop.Services.CouponAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CouponDto, Coupon>().ReverseMap();

        });
        return mappingConfig;
    }
}