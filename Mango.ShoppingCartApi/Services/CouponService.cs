using Commons;
using Mango.ShoppingCartApi.Dtos;
using Mango.ShoppingCartApi.Models;

namespace Mango.ShoppingCartApi.Services;

public class CouponService : BaseService, ICouponService
{
    private readonly string _baseUrl;

    public CouponService(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BaseService> logger)
        : base(httpClientFactory, httpContextAccessor, logger)
    {
        _baseUrl = $"{configuration["ServiceUrls:CouponApi"]}/api/v1/coupon";
    }

    public Task<Result<CouponDto?>> GetCoupon(string couponCode)
    {
        return Send<CouponDto>(new ApiRequest
        {
            Method = HttpMethod.Get,
            Url = $"{_baseUrl}/{couponCode}"
        });
    }
}