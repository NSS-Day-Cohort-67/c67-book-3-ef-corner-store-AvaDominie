using CornerStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using CornerStore.Models.DTOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core and provides dummy value for testing
builder.Services.AddNpgsql<CornerStoreDbContext>(builder.Configuration["CornerStoreDbConnectionString"] ?? "testing");

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();




//endpoints go here


// cashier

// adds a cashier
app.MapPost("/api/cashiers", (CornerStoreDbContext db, Cashier cashier) =>
{
    db.Cashiers.Add(cashier);
    db.SaveChanges();
    return Results.Created($"/api/cashiers/{cashier.Id}", cashier);
});


// gets all cashiers
app.MapGet("/api/cashiers", (CornerStoreDbContext db) =>
{
    return db.Cashiers
    // allows access to orders
    .Include(c => c.Orders)
    // allows access to order products
        .ThenInclude(o => o.OrderProducts)
    .Select(c => new CashierDTO
    {
        Id = c.Id,
        FirstName = c.FirstName,
        LastName = c.LastName,
        // access orders of CashierId
        Orders = c.Orders.Select(o => new OrderDTO
        {
            Id = o.Id,
            CashierId = o.CashierId,
            Cashier = new CashierDTO
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName
            },
            // access orderProducts of OrderId
            OrderProducts = o.OrderProducts.Select(op => new OrderProductDTO
            {
                Id = op.Id,
                ProductId = op.ProductId,
                Product = new ProductDTO
                {
                    Id = op.Product.Id,
                    ProductName = op.Product.ProductName,
                    Price = op.Product.Price,
                    Brand = op.Product.Brand,
                    CategoryId = op.Product.CategoryId,
                    // access Category of CategoryId
                    Category = new CategoryDTO
                    {
                        Id = op.Product.Category.Id,
                        CategoryName = op.Product.Category.CategoryName
                    }
                },
                OrderId = op.OrderId,
                Quantity = op.Quantity
            }).ToList(),
        }).ToList()
    }).ToList();
});




// product


// get all products
// app.MapGet("/api/products", (CornerStoreDbContext db) =>
// {
//     return db.Products
//     // allows access to categories
//     .Include(c => c.Category)
//     .Select(p => new ProductDTO
//     {
//         Id = p.Id,
//         ProductName = p.ProductName,
//         Price = p.Price,
//         Brand = p.Brand,
//         CategoryId = p.CategoryId,
//         Category = new CategoryDTO
//         {
//             Id = p.Category.Id,
//             CategoryName = p.Category.CategoryName
//         }
//     }).ToList();
// });
app.MapGet("/api/products", (CornerStoreDbContext db, string search) =>
{
    string lowerSearch = search.ToLower();

    return db.Products
    .Include(p => p.Category)
    .Where(p => p.ProductName.ToLower().Contains(lowerSearch) ||
                p.Category.CategoryName.ToLower().Contains(lowerSearch))
    .Select(p => new ProductDTO
    {
        Id = p.Id,
        ProductName = p.ProductName,
        Price = p.Price,
        Brand = p.Brand,
        CategoryId = p.CategoryId,
        Category = new CategoryDTO
        {
            Id = p.Category.Id,
            CategoryName = p.Category.CategoryName
        }
    }).ToList();
});




// add a product
app.MapPost("/api/products", (CornerStoreDbContext db, Product product) =>
{
    db.Products.Add(product);
    db.SaveChanges();
    return Results.Created($"/api/products/{product.Id}", product);
});


// update a product
app.MapPut("/api/products/{id}", (CornerStoreDbContext db, int id, ProductDTO product) =>
{
    Product productToUpdate = db.Products.SingleOrDefault(product => product.Id == id);
    if (productToUpdate == null)
    {
        return Results.NotFound();
    }

    productToUpdate.ProductName = product.ProductName;
    productToUpdate.Price = product.Price;
    productToUpdate.Brand = product.Brand;
    productToUpdate.CategoryId = product.CategoryId;


    db.SaveChanges();
    return Results.NoContent();
});


// order











app.Run();

//don't move or change this!
public partial class Program { }