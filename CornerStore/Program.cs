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

app.MapPost("/api/cashiers", (CornerStoreDbContext db, Cashier cashier) =>
{
    db.Cashiers.Add(cashier);
    db.SaveChanges();
    return Results.Created($"/api/cashiers/{cashier.Id}", cashier);
});


// app.MapGet("/api/cashiers", (CornerStoreDbContext db) => 
// {
//     return db.Cashiers
//     .Select(c => new CashierDTO
//     {
//         Id = c.Id,
//         FirstName = c.FirstName,
//         LastName = c.LastName
//     }).ToList();
// });
app.MapGet("/api/cashiers", (CornerStoreDbContext db) =>
{
    return db.Cashiers
    .Include(c => c.Orders)
        .ThenInclude(o => o.OrderProducts)
    .Select(c => new CashierDTO
    {
        Id = c.Id,
        FirstName = c.FirstName,
        LastName = c.LastName,
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







// order











app.Run();

//don't move or change this!
public partial class Program { }