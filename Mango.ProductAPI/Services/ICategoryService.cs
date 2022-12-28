using Commons;
using Commons.Dtos;

namespace Mango.ProductAPI.Services;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>?>> GetCategories();
}