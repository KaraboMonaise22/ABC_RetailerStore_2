using Azure;
using Azure.Data.Tables;

namespace ABCRetail.Web.Models
{
    public class Customer : ITableEntity
    {
        public string PartitionKey { get; set; } = "Customer";
        public string RowKey { get; set; } = string.Empty;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
