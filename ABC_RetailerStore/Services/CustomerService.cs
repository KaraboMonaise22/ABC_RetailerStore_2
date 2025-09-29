using ABCRetail.Web.Models;
using Azure.Data.Tables;

namespace ABCRetail.Web.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TableClient _tableClient;

        public CustomerService(TableServiceClient tableServiceClient)
        {
            _tableClient = tableServiceClient.GetTableClient("customers");
            _tableClient.CreateIfNotExists();
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = new List<Customer>();
            await foreach (var customer in _tableClient.QueryAsync<Customer>())
            {
                customers.Add(customer);
            }
            return customers;
        }

        public async Task<Customer?> GetCustomerAsync(string customerId)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<Customer>("Customer", customerId);
                return response.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            customer.RowKey = Guid.NewGuid().ToString();
            await _tableClient.AddEntityAsync(customer);
            return customer;
        }

        public async Task<Customer> UpdateCustomerAsync(Customer customer)
        {
            await _tableClient.UpdateEntityAsync(customer, customer.ETag);
            return customer;
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            await _tableClient.DeleteEntityAsync("Customer", customerId);
        }

        public Task<bool> ExistsAnotherCustomerWithSameEmailAsync(string email, string rowKey)
        {
            throw new NotImplementedException();
        }
    }
}
