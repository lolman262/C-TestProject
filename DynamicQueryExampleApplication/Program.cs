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

                // Uncomment the following line to delete the database
                context.Database.EnsureDeleted();


                // Ensure database is created and seed data
                context.Database.EnsureCreated();
                SeedData(context);
                // Define filter and sort criteria
                var filterCriteria = new List<FilterCriteria>{
                new FilterCriteria { PropertyName = "Price", Value = 3.5f },
                new FilterCriteria { PropertyName = "Name", Value = "Milo" }
                };
                var sortCriteria = new List<SortCriteria>{
                new SortCriteria { PropertyName = "Name", SortDirection = SortDirection.Ascending }
                };
                // Apply dynamic filter and sort
                var filteredAndSortedProducts = context.Products
                 .Where(filterCriteria)
                 .OrderBy(sortCriteria)
                .ToList();
                // Display results
                Console.WriteLine("Filtered and sorted products:");
                foreach (var product in filteredAndSortedProducts)
                {
                    Console.WriteLine($"{product.Name}: {product.Price}");
                }

                // Define filter and sort criteria
                var filterCriteriaPerson = new List<FilterCriteria>{
                new FilterCriteria { PropertyName = "BirthDate", Value = new DateTime(1990, 7, 1) }
                };
                var sortCriteriaPerson = new List<SortCriteria>{
                new SortCriteria { PropertyName = "Name", SortDirection = SortDirection.Ascending }
                };
                // Apply dynamic filter and sort
                var filteredAndSortedProductsPerson = context.Customers
                 .Where(filterCriteriaPerson)
                 .OrderBy(sortCriteriaPerson)
                .ToList();
                // Display results
                Console.WriteLine("Filtered and sorted customer:");
                foreach (var product in filteredAndSortedProductsPerson)
                {
                    Console.WriteLine($"{product.Name}: {product.BirthDate}");
                }
            }
        }
        private static void SeedData(ExampleDbContext context)
        {
            var products = new[]
            {
            new Product { Name = "Maggi Mee", Price = 3.5f, CreatedDate = DateTime.Now },
            new Product { Name = "Banana", Price = 2, CreatedDate = DateTime.Now },
            new Product { Name = "Biscuit", Price = 6, CreatedDate = DateTime.Now },
            new Product { Name = "Milo", Price = 3.5f, CreatedDate = DateTime.Now }
            };
            var customers = new[]
            {
            new Customer { Name = "Ming Yong", BirthDate = new DateTime(1990, 7, 1) },
            new Customer { Name = "Wilson Chun", BirthDate = new DateTime(1990, 7, 1) },
            new Customer { Name = "Lim Ah Yong", BirthDate = new DateTime(1980, 5, 10) },
            new Customer { Name = "Tan Jia Sung", BirthDate = new DateTime(1985, 2, 15) }
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
        public float Price { get; set; }
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

            optionsBuilder.UseSqlite("Data Source=example.db");
        }
    }
}