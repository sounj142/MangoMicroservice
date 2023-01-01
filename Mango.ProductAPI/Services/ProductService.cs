using AutoMapper;
using AutoMapper.QueryableExtensions;
using Commons;
using Mango.ProductAPI.Dtos;
using Mango.ProductAPI.Models;
using Mango.ProductAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Mango.ProductAPI.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<ProductDto>?>> GetProducts()
    {
        var products = await _dbContext.Products.AsNoTracking()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return Result<List<ProductDto>>.Success(products);
    }

    public async Task<Result<ProductDto?>> GetProduct(Guid id)
    {
        var product = await _dbContext.Products.AsNoTracking()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
        return product == null
            ? Result<ProductDto>.Failure("Product not found.")
            : Result<ProductDto>.Success(product);
    }

    public async Task<Result<ProductDto?>> CreateProduct(ProductCreateDto product)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == product.CategoryId);
        if (category == null)
            return Result<ProductDto>.Failure("Category not found.");

        var newProduct = _mapper.Map<Product>(product);
        newProduct.Id = Guid.NewGuid();
        newProduct.Category = category;
        _dbContext.Products.Add(newProduct);

        await _dbContext.SaveChangesAsync();
        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(newProduct));
    }

    public async Task<Result<ProductDto?>> UpdateProduct(Guid id, ProductCreateDto product)
    {
        var currentProduct = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id);
        if (currentProduct == null)
            return Result<ProductDto>.Failure("Product not found.");

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == product.CategoryId);
        if (category == null)
            return Result<ProductDto>.Failure("Category not found.");

        _mapper.Map(product, currentProduct);
        currentProduct.Category = category;

        await _dbContext.SaveChangesAsync();
        return Result<ProductDto>.Success(_mapper.Map<ProductDto>(currentProduct));
    }

    public async Task<Result<object?>> DeleteProduct(Guid id)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == id);
        if (product == null)
            return Result<object>.Failure("Product not found.");

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return Result<object>.Success(null);
    }
}