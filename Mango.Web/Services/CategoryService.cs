﻿using Commons;
using Commons.Dtos;
using Mango.Web.Models;

namespace Mango.Web.Services;

public class CategoryService : BaseService, ICategoryService
{
    private readonly string _baseUrl;

    public CategoryService(
        ServiceUrls serviceUrls,
        IHttpClientFactory httpClientFactory,
        ILogger<BaseService> logger)
        : base(httpClientFactory, logger)
    {
        _baseUrl = $"{serviceUrls.ProductApi}/api/v1/categories";
    }

    public Task<Result<List<CategoryDto>?>> GetCategories()
    {
        return Send<List<CategoryDto>>(new ApiRequest
        {
            Method = HttpMethod.Get,
            Url = _baseUrl
        });
    }
}