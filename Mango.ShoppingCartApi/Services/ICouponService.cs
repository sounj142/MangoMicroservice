using Commons;
using Mango.ShoppingCartApi.Dtos;

namespace Mango.ShoppingCartApi.Services;

public interface ICouponService
{
    Task<Result<CouponDto?>> GetCoupon(string couponCode);
}