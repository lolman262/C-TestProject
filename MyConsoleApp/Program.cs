using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            SeedData(context);


// TODO: Allow input of filters and sorts from the user
            var filters = new List<FilterCriterion>
            {
                new FilterCriterion { PropertyName = "Name", Value = "Bob" }
            };

            var sorts = new List<SortCriterion>
            {
                new SortCriterion { PropertyName = "DateOfBirth", SortDirection = SortDirection.Ascending }
            };

            var filteredAndSortedCustomers = context.Customers
                .ApplyFilters(filters)
                .ApplySorting(sorts)
                .ToList();

            foreach (var customer in filteredAndSortedCustomers)
            {
                Console.WriteLine($"{customer.Name} - {customer.DateOfBirth}");
            }
        }
    }

    static void SeedData(AppDbContext context)
    {

// TODO: Allow user to input customer data and order data
        var customers = new List<Customer>
        {
            new Customer { Name = "Alice", DateOfBirth = new DateTime(1985, 1, 1) },
            new Customer { Name = "Bob", DateOfBirth = new DateTime(1990, 1, 1) },
            new Customer { Name = "Bob", DateOfBirth = new DateTime(1990, 1, 1) },
            new Customer { Name = "Charlie", DateOfBirth = new DateTime(1982, 1, 1) }
        };

        context.Customers.AddRange(customers);
        context.SaveChanges();
    //    context.SaveChanges();
    }
}
