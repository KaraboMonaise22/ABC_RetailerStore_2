using ABCRetail.Web.Models;
using Azure.Data.Tables;
using Azure.Storage.Blobs;

namespace ABCRetail.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly TableClient _tableClient;
        private readonly BlobContainerClient _blobContainer;

        public ProductService(TableServiceClient tableServiceClient, BlobServiceClient blobServiceClient)
        {
            _tableClient = tableServiceClient.GetTableClient("products");
            _tableClient.CreateIfNotExists();

            _blobContainer = blobServiceClient.GetBlobContainerClient("product-images");
            _blobContainer.CreateIfNotExists();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();
            await foreach (var product in _tableClient.QueryAsync<Product>())
            {
                products.Add(product);
            }
            return products;
        }

        public async Task<Product?> GetProductAsync(string productId)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<Product>("Product", productId);
                return response.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            product.RowKey = Guid.NewGuid().ToString();
            await _tableClient.AddEntityAsync(product);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            await _tableClient.UpdateEntityAsync(product, product.ETag);
            return product;
        }

        public async Task DeleteProductAsync(string productId)
        {
            await _tableClient.DeleteEntityAsync("Product", productId);
        }

        public async Task<string> UploadProductImageAsync(IFormFile file, string productId)
        {
            var fileName = $"{productId}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = _blobContainer.GetBlobClient(fileName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }
    }
}
