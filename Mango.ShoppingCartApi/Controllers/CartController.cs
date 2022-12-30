using Commons;
using Mango.ShoppingCartApi.Dtos;
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

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<Result<CartHeaderDto?>> GetCart([FromQuery] string userId)
    {
        return await _cartService.GetOrCreateCartByUserId(userId);
    }

    [HttpPost]
    public async Task<Result<CartHeaderDto?>> AddToCart(AddToCartDto model)
    {
        return await _cartService.CreateOrUpdateCart(model.UserId, model.Count, model.Product);
    }

    [HttpPut]
    public async Task<Result<CartHeaderDto?>> RemoveFromCart(RemoveFromCartDto model)
    {
        return await _cartService.RemoveFromCart(model.UserId, model.ProductId);
    }

    [HttpPost("apply-coupon")]
    public async Task<Result<CartHeaderDto?>> ApplyCoupon(ApplyCouponDto model)
    {
        return await _cartService.ApplyCoupon(model.UserId, model.CouponCode, model.DiscountAmount);
    }

    [HttpDelete]
    public async Task<Result<object?>> ClearCart([FromQuery] string userId)
    {
        return await _cartService.ClearCart(userId);
    }
}