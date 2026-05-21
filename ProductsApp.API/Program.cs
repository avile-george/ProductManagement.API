using AutoMapper;
using ProductsApp.API.Middleware;
using ProductsApp.Application.Helpers;
using ProductsApp.Application.SearchEngines;
using ProductsApp.Application.Services;
using ProductsApp.Data.Repositories;
using ProductsApp.Domain.Interfaces.Repositories;
using ProductsApp.Domain.Interfaces.SearchEngines;
using ProductsApp.Domain.Interfaces.Services;
using ProductsApp.Domain.Mapping;
using ProductsApp.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure CORS to allow requests from http://localhost:4200
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowMyLocalhost4200", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// register repositories
// builder.Services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();

builder.Services.AddSingleton<ICacheHelper, CacheHelper>();

// register services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

// register searching service
builder.Services.AddScoped<IProductSearchEngine, ProductSearchEngine>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCaching(); // categorie
builder.Services.AddMemoryCache(); // products

var app = builder.Build();

// Seed mock data
using (var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;
    var categoryRepo = provider.GetRequiredService<ICategoryRepository>();
    var productRepo = provider.GetRequiredService<IProductRepository>();

    var electronics = categoryRepo.CreateAsync(new Category { Name = "Electronics", Description = "Electronic devices" }).GetAwaiter().GetResult();
    var groceries = categoryRepo.CreateAsync(new Category { Name = "Groceries", Description = "Daily groceries" }).GetAwaiter().GetResult();
    var toiletries = categoryRepo.CreateAsync(new Category { Name = "Toiletries", Description = "Toiletries & Personal Hygiene", ParentCategoryId = groceries.Id }).GetAwaiter().GetResult();
    var skinCare = categoryRepo.CreateAsync(new Category { Name = "Skin Care", Description = "Skin Care Products", ParentCategoryId = toiletries.Id }).GetAwaiter().GetResult();

    productRepo.CreateAsync(new Product { Name = "Smartphone", Description = "Android phone", Sku="AXD156", Quantity = 100, Price = 599.99m,  CategoryId = electronics.Id }).GetAwaiter().GetResult();
    productRepo.CreateAsync(new Product { Name = "Laptop", Description = "Lightweight laptop", Sku = "12389", Quantity = 230, Price = 1299.99m, CategoryId = electronics.Id }).GetAwaiter().GetResult();
    productRepo.CreateAsync(new Product { Name = "Bread", Description = "Whole grain", Sku = "SKU888", Quantity = 48, Price = 2.49m, CategoryId = groceries.Id }).GetAwaiter().GetResult();
    productRepo.CreateAsync(new Product { Name = "Milk", Description = "Dairy", Sku = "TYU3455", Quantity = 12, Price = 2.49m, CategoryId = groceries.Id }).GetAwaiter().GetResult();
    productRepo.CreateAsync(new Product { Name = "Lotion", Description = "Skin Moisturizer", Sku = "675765", Quantity = 212, Price = 9.99m, CategoryId = skinCare.Id }).GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowMyLocalhost4200");
//app.UseAuthorization();

app.UseResponseCaching();

app.MapControllers();

app.UseMiddleware<BasicLoggerMiddleware>();

app.Run();
