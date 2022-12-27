using Commons;
using Mango.Web.Models;
using Mango.Web.Utils;
using System.Text;
using System.Text.Json;

namespace Mango.Web.Services;

public class BaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BaseService> _logger;

    public BaseService(IHttpClientFactory httpClientFactory,
        ILogger<BaseService> logger)
    {
        _httpClientFactory = httpClientFactory;
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
            if (request.Body != null)
            {
                var content = JsonSerializer.Serialize(request.Body);
                httpRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            using var response = await client.SendAsync(httpRequest);
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogWarning($"Error calling product API respond. Status code: {response.StatusCode}, Body: {responseBody}");
                return Result<T>.Failure("Error in http response.", "HttpRequestError");
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