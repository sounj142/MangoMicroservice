using Commons;
using Mango.Web.Dtos;

namespace Mango.Web.Services;

public interface ICouponService
{
    Task<Result<CouponDto?>> GetCoupon(string couponCode);
}