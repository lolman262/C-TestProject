
# Dynamic Query Library
## Introduction
This library allows for dynamically generating and executing EF Core queries based on custom filtering criteria and sorting instructions. The filtering and sorting logic are implemented using Expression Trees to ensure type safety and performance.
## Features
- Dynamic Query Construction
- Support for Multiple Data Types
- Expression Tree-based Filtering and Sorting
- Generic Implementation
- User-Friendly API


### Getting Started

To get started with the library, follow these steps:

1. Clone the repository or download the source code.
2. Reference the Class Library to the application.
```
dotnet add reference Path\To\Library\DynamicQueryLibrary.csproj
```
4. Import the DynamicQueryLibrary into your project by adding the following line to your project's dependencies:
```csharp
using MyClassLDynamicQueryLibraryibrary;
```
4. Build and run the application.


## Usage
### Step 1: Define Entities
Define your entities as part of a DbContext.
### Step 2: Add Filtering and Sorting Criteria
Define filtering and sorting criteria using `FilterCriteria` and `SortCriteria`.
### Step 3: Use Extension Methods
Use the extension methods `Where` and `OrderBy` to apply filtering and sorting.
```csharp
var filterCriteria = new List<FilterCriteria>
{
new FilterCriteria { PropertyName = "Price", Value = 100m }
};
var sortCriteria = new List<SortCriteria>
{
new SortCriteria { PropertyName = "Name", SortDirection = SortDirection.Ascending }
};
var filteredAndSortedProducts = context.Products
.Where(filterCriteria)
.OrderBy(sortCriteria)
.ToList();
foreach (var product in filteredAndSortedProducts)
{
Console.WriteLine($"{product.Name}: {product.Price}");
}
```
## Classes
- **FilterCriteria:** Represents a filtering criterion with a property name and value.
- **SortCriteria:** Represents a sorting criterion with a property name and direction (`Ascending` or `Descending`).


## Example Application
The example application demonstrates how to use the FilterCriteria and SortCriteria classes to filter and sort a list of products dynamically. It uses Sqlite database and contains two tables: 'Customers' and 'Products'. 