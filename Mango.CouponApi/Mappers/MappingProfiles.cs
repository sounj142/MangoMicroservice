using AutoMapper;
using Mango.CouponApi.Dtos;
using Mango.CouponApi.Models;

namespace Mango.CouponApi.Mappers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Coupon, CouponDto>().ReverseMap();
    }
}