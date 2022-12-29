using AutoMapper;
using Mango.Web.Dtos;
using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMapper _mapper;

        public HomeController(
            IProductService productService,
            ICartService cartService,
            ICurrentUserContext currentUserContext,
            IMapper mapper)
        {
            _productService = productService;
            _cartService = cartService;
            _currentUserContext = currentUserContext;
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
            var userId = _currentUserContext.GetCurrentUserId();

            var cartResult = await _cartService.GetOrCreateCartByUserId(userId!);
            if (!cartResult.Succeeded)
                return RedirectToAction("Index", "Error",
                    new { message = cartResult.Messages.FirstOrDefault() });

            var productInCart = cartResult.Data!.CartDetails
                .FirstOrDefault(x => x.ProductId == model.Id);
            if (productInCart != null)
                model.Count = productInCart.Count;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            var productResult = await _productService.GetProduct(dto.Id);
            if (!productResult.Succeeded)
                return RedirectToAction("Index", "Error",
                    new { message = productResult.Messages.FirstOrDefault() });

            var model = _mapper.Map<ProductDetailsDto>(productResult.Data);
            model.Count = dto.Count;
            var userId = _currentUserContext.GetCurrentUserId();

            var addToCartResult = await _cartService.CreateOrUpdateCart(
                userId!, dto.Count, productResult.Data!);
            if (!addToCartResult.Succeeded)
                return RedirectToAction("Index", "Error",
                    new { message = addToCartResult.Messages.FirstOrDefault() });

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}