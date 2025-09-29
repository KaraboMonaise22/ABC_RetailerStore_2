using ABCRetail.Web.Models;
using ABCRetail.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetail.Web.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _customerService.CreateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Customer customer)
        {
            if (id != customer.RowKey)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _customerService.UpdateCustomerAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
