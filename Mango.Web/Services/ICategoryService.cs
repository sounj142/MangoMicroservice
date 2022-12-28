using Commons;
using Mango.Web.Dtos;

namespace Mango.Web.Services;

public interface ICategoryService
{
    Task<Result<List<CategoryDto>?>> GetCategories();
}