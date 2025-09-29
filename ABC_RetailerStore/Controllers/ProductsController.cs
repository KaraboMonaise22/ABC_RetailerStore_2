using ABCRetail.Web.Models;
using ABCRetail.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var createdProduct = await _productService.CreateProductAsync(product);

                if (imageFile != null && imageFile.Length > 0)
                {
                    var imageUrl = await _productService.UploadProductImageAsync(imageFile, createdProduct.RowKey);
                    createdProduct.ImageUrl = imageUrl;
                    await _productService.UpdateProductAsync(createdProduct);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Product product, IFormFile? imageFile)
        {
            if (id != product.RowKey)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var imageUrl = await _productService.UploadProductImageAsync(imageFile, product.RowKey);
                    product.ImageUrl = imageUrl;
                }

                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
