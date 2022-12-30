using AutoMapper;
using AutoMapper.QueryableExtensions;
using Commons;
using Mango.ShoppingCartApi.Dtos;
using Mango.ShoppingCartApi.Messages;
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
        await _dbContext.SaveChangesAsync();

        var cart = await _dbContext.CartHeaders
            .Include(c => c.CartDetails!)
            .ThenInclude(g => g.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (cart == null)
            cart = await CreateNewCart(userId);

        // add or update cart item
        var cartDetail = cart.CartDetails!.FirstOrDefault(x => x.ProductId == product.Id);
        if (cartDetail == null)
        {
            cartDetail = new CartDetails
            {
                Id = Guid.NewGuid(),
                Count = count,
                Product = currentProduct,
                CartHeader = cart
            };
            _dbContext.CartDetails.Add(cartDetail);
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

    public async Task<Result<CartHeaderDto?>> ApplyCoupon(string userId, string? couponCode, double discountAmount)
    {
        var cart = await _dbContext.CartHeaders
            .Include(c => c.CartDetails!)
            .ThenInclude(g => g.Product)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        if (cart == null)
            return Result<CartHeaderDto>.Failure("Cart not found.");

        cart.CouponCode = couponCode;
        cart.DiscountAmount = discountAmount;
        CalculateCartPrices(cart);
        await _dbContext.SaveChangesAsync();

        return Result<CartHeaderDto>.Success(_mapper.Map<CartHeaderDto>(cart));
    }

    private void CalculateCartPrices(CartHeader cart)
    {
        if (cart.CartDetails!.Count == 0)
        {
            cart.TotalPrice = 0;
            cart.FinalPrice = 0;
            cart.DiscountAmount = 0;
            cart.ActualDiscountAmount = 0;
            cart.CouponCode = null;
        }
        else
        {
            cart.TotalPrice = cart.CartDetails!.Sum(x => x.Count * x.Product!.Price);
            cart.FinalPrice = cart.TotalPrice - cart.DiscountAmount;
            if (cart.FinalPrice < 0) cart.FinalPrice = 0;
            cart.ActualDiscountAmount = cart.TotalPrice - cart.FinalPrice;
        }
    }

    public async Task<Result<object?>> Checkout(CheckoutDto model)
    {
        //model.Cart = GetOrCreateCartByUserId()
        return null;
    }
}