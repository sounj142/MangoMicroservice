using Commons;
using Mango.ProductAPI.Dtos;

namespace Mango.ProductAPI.Services;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>?>> GetCategories();
}