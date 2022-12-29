namespace Mango.Web.Models;

public class ApiRequest
{
    public HttpMethod Method { get; set; } = HttpMethod.Get;

    public string Url { get; set; } = string.Empty;
    public object? Params { get; set; }

    public object? Body { get; set; }
}