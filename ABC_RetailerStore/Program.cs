using ABCRetail.Web.Services;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("AzureStorage");

// Register Azure Storage services
builder.Services.AddSingleton(new TableServiceClient(connectionString));
builder.Services.AddSingleton(new BlobServiceClient(connectionString));
builder.Services.AddSingleton(new QueueServiceClient(connectionString));
builder.Services.AddSingleton(new ShareServiceClient(connectionString));

// Register custom services
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["ConnectionStrings:AzureStorage1:blobServiceUri"]!).WithName("ConnectionStrings:AzureStorage1");
    clientBuilder.AddQueueServiceClient(builder.Configuration["ConnectionStrings:AzureStorage1:queueServiceUri"]!).WithName("ConnectionStrings:AzureStorage1");
    clientBuilder.AddTableServiceClient(builder.Configuration["ConnectionStrings:AzureStorage1:tableServiceUri"]!).WithName("ConnectionStrings:AzureStorage1");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
