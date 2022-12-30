using Commons;
using Mango.CouponApi.Dtos;

namespace Mango.CouponApi.Services;

public interface ICouponService
{
    Task<Result<CouponDto?>> GetCoupon(string couponCode);
}