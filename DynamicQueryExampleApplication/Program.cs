using System;
using System.Linq;
using DynamicQueryLibrary;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace DynamicQueryExample
{
    class Program
    {
        static void Main()
        {
            using (var context = new ExampleDbContext())
            {
                // Ensure database is created and seed data
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedData(context);
                // Define filter and sort criteria
                var filterCriteria = new List<FilterCriteria>
{
new FilterCriteria { PropertyName = "Price", Value = 100m }
};
                var sortCriteria = new List<SortCriteria>
{
new SortCriteria { PropertyName = "Name", SortDirection = SortDirection.Ascending }
};
                // Apply dynamic filter and sort
                var filteredAndSortedProducts = context.Products
                .Where(filterCriteria)
                .OrderBy(sortCriteria)
                .ToList();
                // Display results
                foreach (var product in filteredAndSortedProducts)
                {
                    Console.WriteLine($"{product.Name}: {product.Price}");
                }
            }
        }
        private static void SeedData(ExampleDbContext context)
        {
            var products = new[]
            {
new Product { Name = "Apple", Price = 200m, CreatedDate = DateTime.Now },
new Product { Name = "Banana", Price = 100m, CreatedDate = DateTime.Now },
new Product { Name = "Cherry", Price = 150m, CreatedDate = DateTime.Now }
};
            var customers = new[]
            {
new Customer { Name = "John Doe", BirthDate = new DateTime(1990, 1, 1) },
new Customer { Name = "Jane Doe", BirthDate = new DateTime(1985, 2, 15) }
};
            context.Products.AddRange(products);
            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
    public class ExampleDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("ExampleDb");
            optionsBuilder.UseSqlite("Data Source=example.db");
        }
    }
}