using Mango.IdentityServer.UI;
using System.ComponentModel.DataAnnotations;

namespace Mango.IdentityServer.Account;

public class RegisterViewModel : LoginInputModel
{
    [Required]
    public string Email { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;

    public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
    public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));

    public bool IsExternalLoginOnly => EnableLocalLogin == false && ExternalProviders?.Count() == 1;
    public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders?.SingleOrDefault()?.AuthenticationScheme : null;
}