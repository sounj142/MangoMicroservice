using AutoMapper;
using AutoMapper.QueryableExtensions;
using Commons;
using Mango.ProductAPI.DbContexts;
using Mango.ProductAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.ProductAPI.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<List<CategoryDto>?>> GetCategories()
    {
        var categories = await _dbContext.Categories.AsNoTracking()
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return Result<List<CategoryDto>>.Success(categories);
    }
}