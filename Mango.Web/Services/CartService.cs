﻿using Commons;
using Mango.Web.Dtos;
using Mango.Web.Models;

namespace Mango.Web.Services;

public class CartService : BaseService, ICartService
{
    private readonly string _baseUrl;

    public CartService(
        ServiceUrls serviceUrls,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BaseService> logger)
        : base(httpClientFactory, httpContextAccessor, logger)
    {
        _baseUrl = $"{serviceUrls.ShoppingCartApi}/api/v1/cart";
    }

    public Task<Result<CartHeaderDto?>> GetOrCreateCartByUserId(string userId)
    {
        return Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Get,
            Url = _baseUrl,
            Params = new { userId }
        });
    }

    public Task<Result<CartHeaderDto?>> CreateOrUpdateCart(string userId, int count, ProductDto product)
    {
        return Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Post,
            Url = _baseUrl,
            Body = new CreateOrUpdateCart
            {
                UserId = userId,
                Count = count,
                Product = product
            }
        });
    }

    public Task<Result<CartHeaderDto?>> RemoveFromCart(string userId, Guid productId)
    {
        return Send<CartHeaderDto>(new ApiRequest
        {
            Method = HttpMethod.Put,
            Url = _baseUrl,
            Body = new RemoveFromCartDto
            {
                UserId = userId,
                ProductId = productId
            }
        });
    }

    public Task<Result<object?>> ClearCart(string userId)
    {
        return Send<object>(new ApiRequest
        {
            Method = HttpMethod.Delete,
            Url = _baseUrl,
            Params = new { userId }
        });
    }
}