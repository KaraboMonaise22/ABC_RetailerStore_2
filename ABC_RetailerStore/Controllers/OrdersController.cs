using ABCRetail.Web.Models;
using ABCRetail.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public OrdersController(IOrderService orderService, ICustomerService customerService, IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _orderService.GetOrderMessagesAsync();
            return View(messages);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Products = await _productService.GetAllProductsAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderMessage orderMessage)
        {
            if (ModelState.IsValid)
            {
                orderMessage.OrderId = Guid.NewGuid().ToString();
                orderMessage.Status = "Processing";
                await _orderService.ProcessOrderAsync(orderMessage);
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Products = await _productService.GetAllProductsAsync();
            return View(orderMessage);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // For demo purposes, create a sample order message
            var orderMessage = new OrderMessage 
            { 
                OrderId = id,
                CustomerId = "sample-customer",
                ProductId = "sample-product", 
                Quantity = 1,
                TotalAmount = 99.99m,
                Status = "Processing",
                CreatedDate = DateTime.UtcNow
            };

            return View(orderMessage);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderMessage = new OrderMessage 
            { 
                OrderId = id,
                CustomerId = "sample-customer",
                ProductId = "sample-product", 
                Quantity = 1,
                TotalAmount = 99.99m,
                Status = "Processing",
                CreatedDate = DateTime.UtcNow
            };

            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Products = await _productService.GetAllProductsAsync();
            return View(orderMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, OrderMessage orderMessage)
        {
            if (id != orderMessage.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update order logic would go here
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Products = await _productService.GetAllProductsAsync();
            return View(orderMessage);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderMessage = new OrderMessage 
            { 
                OrderId = id,
                CustomerId = "sample-customer",
                ProductId = "sample-product", 
                Quantity = 1,
                TotalAmount = 99.99m,
                Status = "Processing",
                CreatedDate = DateTime.UtcNow
            };

            return View(orderMessage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Delete order logic would go here
            return RedirectToAction(nameof(Index));
        }
    }
}
