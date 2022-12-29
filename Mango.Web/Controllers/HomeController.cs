using AutoMapper;
using Mango.Web.Dtos;
using Mango.Web.Models;
using Mango.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public HomeController(
            IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var productsResult = await _productService.GetProducts();
            var products = productsResult.Data ?? new List<ProductDto>();
            return View(products);
        }

        public async Task<IActionResult> Details(string id)
        {
            var productResult = await _productService.GetProduct(id);
            return View(_mapper.Map<ProductDetailsDto>(productResult.Data));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}