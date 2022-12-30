using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

public class AccountController : Controller
{
    [Authorize]
    public IActionResult Login()
    {
        return RedirectToAction(nameof(Index), "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}