using Commons;
using Commons.Services;
using Mango.ShoppingCartApi.Dtos;
using Mango.ShoppingCartApi.Messages;
using Mango.ShoppingCartApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.ShoppingCartApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class CartController
{
    private readonly ICartService _cartService;
    private readonly ICurrentUserContext _currentUserContext;

    public CartController(
        ICartService cartService,
        ICurrentUserContext currentUserContext)
    {
        _cartService = cartService;
        _currentUserContext = currentUserContext;
    }

    [HttpGet]
    public async Task<Result<CartHeaderDto?>> GetCart()
    {
        return await _cartService.GetOrCreateCartByUserId(_currentUserContext.GetCurrentUserId()!);
    }

    [HttpPost]
    public async Task<Result<CartHeaderDto?>> AddToCart(AddToCartDto model)
    {
        return await _cartService.CreateOrUpdateCart(_currentUserContext.GetCurrentUserId()!
            , model.Count, model.Product);
    }

    [HttpPut]
    public async Task<Result<CartHeaderDto?>> RemoveFromCart(RemoveFromCartDto model)
    {
        return await _cartService.RemoveFromCart(
            _currentUserContext.GetCurrentUserId()!, model.ProductId);
    }

    [HttpPost("apply-coupon")]
    public async Task<Result<CartHeaderDto?>> ApplyCoupon(ApplyCouponDto model)
    {
        return await _cartService.ApplyCoupon(_currentUserContext.GetCurrentUserId()!,
            model.CouponCode, model.DiscountAmount);
    }

    [HttpDelete]
    public async Task<Result<object?>> ClearCart()
    {
        return await _cartService.ClearCart(_currentUserContext.GetCurrentUserId()!);
    }

    [HttpPost("checkout")]
    public async Task<Result<object?>> Checkout(CheckoutDto model)
    {
        return await _cartService.Checkout(model);
    }
}