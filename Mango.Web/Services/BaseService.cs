using Commons;
using Mango.Web.Models;
using Mango.Web.Utils;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Net;

namespace Mango.Web.Services;

public class BaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BaseService> _logger;

    public BaseService(
        IHttpClientFactory httpClientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BaseService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Result<T?>> Send<T>(ApiRequest request)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Clear();

            var url = request.Url;
            if (request.Params != null) url = url.AddQueryString(request.Params);
            var httpRequest = new HttpRequestMessage(request.Method, url);
            httpRequest.Headers.Add("Accept", "application/json");
            if (_httpContextAccessor.HttpContext != null)
            {
                var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            if (request.Body != null)
            {
                var content = JsonSerializer.Serialize(request.Body);
                httpRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            using var response = await client.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning($"Error calling product API respond. Status code: {(int)response.StatusCode}, Body: {responseBody}");
                var message = response.StatusCode == HttpStatusCode.Forbidden ?
                    "You don't have permission to do this action." : "Error in Http response.";
                return Result<T>.Failure(message, "HttpRequestError");
            }

            var result = await response.Content.ReadFromJsonAsync<Result<T?>>();
            return result!;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error happened when sending http request.", ex);
            return Result<T>.Failure("Error happened when sending http request.", "HttpRequestError");
        }
    }
}