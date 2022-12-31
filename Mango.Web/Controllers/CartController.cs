using AutoMapper;
using Commons.Services;
using Mango.Web.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICouponService _couponService;
    private readonly IMapper _mapper;

    public CartController(
        ICartService cartService,
        ICouponService couponService,
        IMapper mapper)
    {
        _cartService = cartService;
        _couponService = couponService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var cartResult = await _cartService.GetOrCreateCartOfCurrentUser();
        return View(cartResult.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove([FromForm] Guid productId)
    {
        await _cartService.RemoveFromCart(new RemoveFromCartDto { ProductId = productId });

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
            CouponCode = null,
            DiscountAmount = 0
        };
        await _cartService.ApplyCoupon(applyCouponDto);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var cartResult = await _cartService.GetOrCreateCartOfCurrentUser();
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
        var checkout = new CheckoutDto
        {
            Cart = cartResult.Data,
            CouponCode = cartResult.Data.CouponCode,
            TotalPrice = cartResult.Data.TotalPrice,
            DiscountAmount = cartResult.Data.DiscountAmount,
            ActualDiscountAmount = cartResult.Data.ActualDiscountAmount,
            FinalPrice = cartResult.Data.FinalPrice,
        };

        return View(checkout);
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutDto checkout)
    {
        var cartResult = await _cartService.GetOrCreateCartOfCurrentUser();
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

        checkout.Cart = cartResult.Data;
        var checkoutResult = await _cartService.Checkout(checkout);
        if (!checkoutResult.Succeeded)
        {
            TempData["ErrorMessage"] = checkoutResult.Messages.FirstOrDefault();
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Confirmation));
    }

    public IActionResult Confirmation()
    {
        return View();
    }
}