using Commons;
using Commons.Dtos;
using Mango.ProductAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mango.CategoryAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<Result<List<CategoryDto>?>> GetCategories()
        {
            return await _categoryService.GetCategories();
        }
    }
}