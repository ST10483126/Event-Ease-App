using Azure.Storage.Blobs;
using EventEaseApp.Data;
using EventEaseApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// 1. Register DbContext with SQL Server
builder.Services.AddDbContext<EventEaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventEaseContext")
        ?? throw new InvalidOperationException("Connection string 'EventEaseContext' not found.")));

// 2. Register BlobServiceClient (for Azure Blob Storage)
builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")
        ?? throw new InvalidOperationException("Connection string 'AzureBlobStorage' not found.")));

// 3. Register your custom BlobService
builder.Services.AddScoped<BlobService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// .NET 10 recommended approach for static files
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();