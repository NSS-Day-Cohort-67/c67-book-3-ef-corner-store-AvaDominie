using Microsoft.EntityFrameworkCore;
using CornerStore.Models;
public class CornerStoreDbContext : DbContext
{
    public DbSet<Cashier> Cashiers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    public DbSet<Product> Products { get; set; }


    public CornerStoreDbContext(DbContextOptions<CornerStoreDbContext> context) : base(context)
    {

    }

    //allows us to configure the schema when migrating as well as seed data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // seed data


        // cashier
        modelBuilder.Entity<Cashier>().HasData(new Cashier[]
        {
            new Cashier {Id = 1, FirstName = "Ava", LastName = "Dominie"},
            new Cashier {Id = 2, FirstName = "Rachel", LastName = "Brewer"},
            new Cashier {Id = 3, FirstName = "Tom", LastName = "Mounth"}
        });


        // category
        modelBuilder.Entity<Category>().HasData(new Category[]
        {
            new Category {Id = 1, CategoryName = "Food"},
            new Category {Id = 2, CategoryName = "Drink"},
            new Category {Id = 3, CategoryName = "Candy"}
        });


        // order
        modelBuilder.Entity<Order>().HasData(new Order[]
        {
            new Order {Id = 1, CashierId = 1, PaidOnDate = new DateTime(2023, 12, 5)},
            new Order {Id = 2, CashierId = 2, PaidOnDate = new DateTime(2023, 12, 19)},
            new Order {Id = 3, CashierId = 3, PaidOnDate = new DateTime(2023, 12, 1)}
        });


        // order product
        modelBuilder.Entity<OrderProduct>().HasData(new OrderProduct[]
        {
            new OrderProduct {Id = 1, ProductId = 2, OrderId = 1, Quantity = 2},
            new OrderProduct {Id = 2, ProductId = 2, OrderId = 3, Quantity = 2},
            new OrderProduct {Id = 3, ProductId = 1, OrderId = 2, Quantity = 4}
        });


        // product
        modelBuilder.Entity<Product>().HasData(new Product[]
        {
            new Product {Id = 1, ProductName = "Kit-kat", Price = 1.50M, Brand = "Nestlé", CategoryId = 3},
            new Product {Id = 2, ProductName = "Pizza Hot Pocket", Price = 2.75M, Brand = "Hot Pockets", CategoryId = 1},
            new Product {Id = 3, ProductName = "Tea", Price = 1.00M, Brand = "lisbon", CategoryId = 2 }
        });
    }
}

// dotnet ef migrations add InitialCreate
// dotnet ef database update