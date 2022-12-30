using AutoMapper;
using AutoMapper.QueryableExtensions;
using Commons;
using Mango.CouponApi.Dtos;
using Mango.CouponApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mango.CouponApi.Services;

public class CouponService : ICouponService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CouponService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<CouponDto?>> GetCoupon(string couponCode)
    {
        var coupon = await _dbContext.Coupons.AsNoTracking()
            .ProjectTo<CouponDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.CouponCode == couponCode);
        return coupon == null
           ? Result<CouponDto>.Failure("Coupon not found.")
           : Result<CouponDto>.Success(coupon);
    }
}