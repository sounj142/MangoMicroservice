using AutoMapper;
using Mango.Web.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICurrentUserContext _userContext;
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;
    private readonly IMapper _mapper;

    public CartController(
        ICurrentUserContext userContext,
        ICartService cartService,
        ICouponService couponService,
        IMapper mapper)
    {
        _userContext = userContext;
        _cartService = cartService;
        _couponService = couponService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userContext.GetCurrentUserId();
        var cartResult = await _cartService.GetOrCreateCartByUserId(userId!);

        return View(cartResult.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove([FromForm] Guid productId)
    {
        var userId = _userContext.GetCurrentUserId();
        await _cartService.RemoveFromCart(new RemoveFromCartDto
        {
            ProductId = productId,
            UserId = userId!
        });

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyCoupon(string couponCode)
    {
        if (string.IsNullOrEmpty(couponCode))
        {
            TempData["ErrorMessage"] = "Coupon not found.";
            return RedirectToAction(nameof(Index));
        }

        var couponResult = await _couponService.GetCoupon(couponCode);
        if (!couponResult.Succeeded)
        {
            TempData["ErrorMessage"] = couponResult.Messages.FirstOrDefault();
            return RedirectToAction(nameof(Index));
        }

        var coupon = couponResult.Data!;
        var applyCouponDto = new ApplyCouponDto
        {
            UserId = _userContext.GetCurrentUserId()!,
            CouponCode = coupon.CouponCode,
            DiscountAmount = coupon.DiscountAmount
        };
        await _cartService.ApplyCoupon(applyCouponDto);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveCoupon()
    {
        var applyCouponDto = new ApplyCouponDto
        {
            UserId = _userContext.GetCurrentUserId()!,
            CouponCode = null,
            DiscountAmount = 0
        };
        await _cartService.ApplyCoupon(applyCouponDto);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Checkout()
    {
        var userId = _userContext.GetCurrentUserId();
        var cartResult = await _cartService.GetOrCreateCartByUserId(userId!);
        if (!cartResult.Succeeded)
        {
            TempData["ErrorMessage"] = cartResult.Messages.FirstOrDefault();
            return RedirectToAction("Index", "Home");
        }
        if (cartResult.Data == null || cartResult.Data.CartDetails.Count == 0)
        {
            TempData["ErrorMessage"] = "You can't checkout because your cart is empty.";
            return RedirectToAction(nameof(Index));
        }
        var order = new CheckoutDto
        {
            Cart = cartResult.Data,
            CouponCode = cartResult.Data.CouponCode,
            TotalPrice = cartResult.Data.TotalPrice,
            DiscountAmount = cartResult.Data.DiscountAmount,
            ActualDiscountAmount = cartResult.Data.ActualDiscountAmount,
            FinalPrice = cartResult.Data.FinalPrice,
        };

        return View(order);
    }
}