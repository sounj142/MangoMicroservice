using Commons;
using Mango.Web.Dtos;
using Mango.Web.Models;

namespace Mango.Web.Services;

public class ProductService : BaseService, IProductService
{
    private readonly string _baseUrl;

    public ProductService(
        ServiceUrls serviceUrls,
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BaseService> logger)
        : base(httpClientFactory, httpContextAccessor, logger)
    {
        _baseUrl = $"{serviceUrls.ProductApi}/api/v1/products";
    }

    public Task<Result<List<ProductDto>?>> GetProducts()
    {
        return Send<List<ProductDto>>(new ApiRequest
        {
            Method = HttpMethod.Get,
            Url = _baseUrl
        });
    }

    public Task<Result<ProductDto?>> GetProduct(string id)
    {
        return Send<ProductDto>(new ApiRequest
        {
            Method = HttpMethod.Get,
            Url = $"{_baseUrl}/{id}",
        });
    }

    public Task<Result<ProductDto?>> CreateProduct(ProductCreateDto product)
    {
        return Send<ProductDto>(new ApiRequest
        {
            Method = HttpMethod.Post,
            Url = _baseUrl,
            Body = product
        });
    }

    public Task<Result<ProductDto?>> UpdateProduct(ProductUpdateDto product)
    {
        return Send<ProductDto>(new ApiRequest
        {
            Method = HttpMethod.Put,
            Url = $"{_baseUrl}/{product.Id}",
            Body = product
        });
    }

    public Task<Result<object?>> DeleteProduct(string id)
    {
        return Send<object>(new ApiRequest
        {
            Method = HttpMethod.Delete,
            Url = $"{_baseUrl}/{id}",
        });
    }
}