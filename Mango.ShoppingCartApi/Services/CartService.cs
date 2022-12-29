using AutoMapper;
using AutoMapper.QueryableExtensions;
using Commons;
using Mango.ShoppingCartApi.Dtos;
using Mango.ShoppingCartApi.Models;
using Mango.ShoppingCartApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mango.ShoppingCartApi.Services;

public class CartService : ICartService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CartService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    private async Task<CartHeader> CreateNewCart(string userId)
    {
        var cart = new CartHeader
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CartDetails = new List<CartDetails>()
        };
        CalculateCartPrices(cart);
        _dbContext.CartHeaders.Add(cart);
        await _dbContext.SaveChangesAsync();
        return cart;
    }

    public async Task<Result<CartHeaderDto?>> GetOrCreateCartByUserId(string userId)
    {
        var cart = await _dbContext.CartHeaders.AsNoTracking()
            .ProjectTo<CartHeaderDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (cart == null)
        {
            var newCart = await CreateNewCart(userId);
            cart = _mapper.Map<CartHeaderDto>(newCart);
        }
        return Result<CartHeaderDto>.Success(cart);
    }

    public async Task<Result<CartHeaderDto?>> CreateOrUpdateCart(
        string userId, int count, ProductDto product)
    {
        if (count < 1)
            return await RemoveFromCart(userId, product.Id);

        var cart = await _dbContext.CartHeaders
            .Include(c => c.CartDetails!)
            .ThenInclude(g => g.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (cart == null)
            cart = await CreateNewCart(userId);

        // add or update product
        var currentProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
        if (currentProduct == null)
        {
            currentProduct = _mapper.Map<Product>(product);
            _dbContext.Products.Add(currentProduct);
        }
        else
        {
            _mapper.Map(product, currentProduct);
        }

        // add or update cart item
        var cartDetail = cart.CartDetails!.FirstOrDefault(x => x.ProductId == product.Id);
        if (cartDetail == null)
        {
            cartDetail = new CartDetails
            {
                Id = Guid.NewGuid(),
                Count = count,
                ProductId = currentProduct.Id,
                Product = currentProduct
            };
            cart.CartDetails!.Add(cartDetail);
        }
        else
        {
            cartDetail.Count = count;
        }

        CalculateCartPrices(cart);
        await _dbContext.SaveChangesAsync();

        return Result<CartHeaderDto>.Success(_mapper.Map<CartHeaderDto>(cart));
    }

    public async Task<Result<CartHeaderDto?>> RemoveFromCart(string userId, Guid productId)
    {
        var cart = await _dbContext.CartHeaders
            .Include(c => c.CartDetails!)
            .ThenInclude(g => g.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (cart == null)
            return Result<CartHeaderDto>.Failure("Cart not found.");

        cart.CartDetails = cart.CartDetails!
            .Where(x => x.ProductId != productId)
            .ToList();

        CalculateCartPrices(cart);
        await _dbContext.SaveChangesAsync();

        return Result<CartHeaderDto>.Success(_mapper.Map<CartHeaderDto>(cart));
    }

    public async Task<Result<object?>> ClearCart(string userId)
    {
        var cart = await _dbContext.CartHeaders
            .Include(c => c.CartDetails!)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (cart == null)
            return Result<object>.Failure("Cart not found.");

        cart.CartDetails!.Clear();
        cart.CouponCode = null;
        CalculateCartPrices(cart);
        await _dbContext.SaveChangesAsync();

        return Result<object>.Success(null);
    }

    public async Task<Result<CartHeaderDto?>> ApplyCoupon(string userId, string? couponCode)
    {
        var cart = await _dbContext.CartHeaders
            .Include(c => c.CartDetails!)
            .ThenInclude(g => g.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (cart == null)
            return Result<CartHeaderDto>.Failure("Cart not found.");

        cart.CouponCode = couponCode;
        CalculateCartPrices(cart);
        await _dbContext.SaveChangesAsync();

        return Result<CartHeaderDto>.Success(_mapper.Map<CartHeaderDto>(cart));
    }

    private void CalculateCartPrices(CartHeader cart)
    {
        cart.TotalPrice = cart.CartDetails!.Sum(x => x.Count * x.Product!.Price);
        cart.TotalCoupon = 0;
        if (cart.CouponCode == "10%OFF")
            cart.TotalCoupon = cart.TotalPrice * 10.0 / 100;
        cart.FinalPrice = cart.TotalPrice - cart.TotalCoupon;
    }
}