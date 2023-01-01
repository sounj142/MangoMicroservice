using AutoMapper;
using Mango.Web.Dtos;
using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public HomeController(
        IProductService productService,
        ICartService cartService,
        IMapper mapper)
    {
        _productService = productService;
        _cartService = cartService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var productsResult = await _productService.GetProducts();
        var products = productsResult.Data ?? new List<ProductDto>();
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(string id)
    {
        var productResult = await _productService.GetProduct(id);
        if (!productResult.Succeeded)
            return RedirectToAction("Index", "Error",
                new { message = productResult.Messages.FirstOrDefault() });

        var model = _mapper.Map<ProductDetailsDto>(productResult.Data);

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart([FromForm] string id, [FromForm] int count)
    {
        var productResult = await _productService.GetProduct(id);
        if (!productResult.Succeeded)
            return RedirectToAction("Index", "Error",
                new { message = productResult.Messages.FirstOrDefault() });

        var model = _mapper.Map<ProductDetailsDto>(productResult.Data);
        model.Count = count;

        var addToCartResult = await _cartService.CreateOrUpdateCart(
            new CreateOrUpdateCart
            {
                Count = count,
                Product = productResult.Data!
            });
        if (!addToCartResult.Succeeded)
            return RedirectToAction("Index", "Error",
                new { message = addToCartResult.Messages.FirstOrDefault() });

        return RedirectToAction("Index", "Cart");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}