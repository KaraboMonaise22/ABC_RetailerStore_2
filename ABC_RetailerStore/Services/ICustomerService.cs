using ABCRetail.Web.Models;

namespace ABCRetail.Web.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerAsync(string customerId);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string customerId);
        Task<bool> ExistsAnotherCustomerWithSameEmailAsync(string email, string rowKey);
    }
}
