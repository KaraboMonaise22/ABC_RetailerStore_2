namespace ABCRetail.Web.Models
{
    public class OrderMessage
    {
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
