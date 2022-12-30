namespace Commons.Services;

public interface ICurrentUserContext
{
    string? GetCurrentUserId();

    string? GetCurrentUserName();

    string? GetCurrentUserEmail();
}