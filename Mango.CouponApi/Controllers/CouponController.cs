using Commons;
using Mango.CouponApi.Dtos;
using Mango.CouponApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mango.CouponApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    [HttpGet("{couponCode}")]
    public async Task<Result<CouponDto?>> GetCoupon(string couponCode)
    {
        return await _couponService.GetCoupon(couponCode);
    }
}