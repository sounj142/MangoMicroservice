using Commons;
using Commons.Dtos;
using Mango.ProductAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mango.ProductAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        protected async Task<Result<T?>> ParseGuid<T>(string id, Func<Guid, Task<Result<T?>>> actionFunc)
        {
            if (Guid.TryParse(id, out Guid result))
            {
                return await actionFunc(result);
            }
            else return Result<T>.Failure("Product not found.");
        }

        [HttpGet]
        public async Task<Result<List<ProductDto>?>> GetProducts()
        {
            return await _productService.GetProducts();
        }

        [HttpGet("{id}")]
        public Task<Result<ProductDto?>> GetProduct(string id)
        {
            return ParseGuid<ProductDto?>(id,
                (parsedId) => _productService.GetProduct(parsedId));
        }

        [HttpPost]
        public Task<Result<ProductDto?>> CreateProduct(ProductCreateDto model)
        {
            return _productService.CreateProduct(model);
        }

        [HttpPut("{id}")]
        public Task<Result<ProductDto?>> UpdateProduct(string id, ProductCreateDto model)
        {
            return ParseGuid<ProductDto?>(id,
                (parsedId) => _productService.UpdateProduct(parsedId, model));
        }

        [HttpDelete("{id}")]
        public Task<Result<object?>> DeleteProduct(string id)
        {
            return ParseGuid(id,
                (parsedId) => _productService.DeleteProduct(parsedId));
        }
    }
}