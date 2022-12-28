using Commons.Dtos;
using Commons;

namespace Mango.Web.Services;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>?>> GetCategories();
}