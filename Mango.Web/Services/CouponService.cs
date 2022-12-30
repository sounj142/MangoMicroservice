using Commons;
using Mango.Web.Dtos;
using Mango.Web.Models;

namespace Mango.Web.Services;

public class CouponService : BaseService, ICouponService
{
    private readonly string _baseUrl;

    public CouponService(
        ServiceUrls serviceUrls,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BaseService> logger)
        : base(httpClientFactory, httpContextAccessor, logger)
    {
        _baseUrl = $"{serviceUrls.CouponApi}/api/v1/coupon";
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