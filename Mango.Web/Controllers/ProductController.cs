using AutoMapper;
using Mango.Web.Dtos;
using Mango.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public ProductController(
        IProductService productService,
        ICategoryService categoryService,
        IMapper mapper)
    {
        _productService = productService;
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var productsResult = await _productService.GetProducts();
        return View(productsResult);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryService.GetCategories();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateDto model)
    {
        ViewBag.Categories = await _categoryService.GetCategories();
        if (!ModelState.IsValid)
            return View(model);

        var result = await _productService.CreateProduct(model);
        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = result.Messages.FirstOrDefault();
            return View(model);
        }

        TempData["SuccessMessage"] = "Product created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        ViewBag.Categories = await _categoryService.GetCategories();
        var productResult = await _productService.GetProduct(id);
        if (!productResult.Succeeded)
        {
            return RedirectToAction("Index", "Error",
                new { message = productResult.Messages.FirstOrDefault() });
        }

        return View(productResult.Data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductDto model)
    {
        ViewBag.Categories = await _categoryService.GetCategories();
        if (!ModelState.IsValid)
            return View(model);

        var result = await _productService.UpdateProduct(_mapper.Map<ProductUpdateDto>(model));
        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = result.Messages.FirstOrDefault();
            return View(model);
        }

        TempData["SuccessMessage"] = "Product updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var result = await _productService.DeleteProduct(id);
        if (!result.Succeeded)
        {
            return RedirectToAction("Index", "Error",
                new { message = result.Messages.FirstOrDefault() });
        }
        return RedirectToAction(nameof(Index));
    }
}