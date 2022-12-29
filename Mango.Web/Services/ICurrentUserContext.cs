namespace Mango.Web.Services;

public interface ICurrentUserContext
{
    string? GetCurrentUserId();

    string? GetCurrentUserName();

    string? GetCurrentUserEmail();
}