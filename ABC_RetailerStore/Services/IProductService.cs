using ABCRetail.Web.Models;

namespace ABCRetail.Web.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductAsync(string productId);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(string productId);
        Task<string> UploadProductImageAsync(IFormFile file, string productId);
        //string GetImageUrl(string imageName);
    }
}
