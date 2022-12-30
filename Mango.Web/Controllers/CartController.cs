using AutoMapper;
using Mango.Web.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICurrentUserContext _userContext;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public CartController(
        ICurrentUserContext userContext,
        ICartService cartService,
        IMapper mapper)
    {
        _userContext = userContext;
        _cartService = cartService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userContext.GetCurrentUserId();
        var cartResult = await _cartService.GetOrCreateCartByUserId(userId!);

        return View(cartResult.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove([FromForm] Guid productId)
    {
        var userId = _userContext.GetCurrentUserId();
        await _cartService.RemoveFromCart(userId!, productId);

        return RedirectToAction(nameof(Index));
    }
}