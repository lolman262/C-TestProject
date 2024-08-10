using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class FilterCriterion
{
    public string PropertyName { get; set; }
    public object Value { get; set; }
}

public class SortCriterion
{
    public string PropertyName { get; set; }
    public SortDirection SortDirection { get; set; }
}

public enum SortDirection
{
    Ascending,
    Descending
}


public static class DynamicQueryBuilder
{
    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, List<FilterCriterion> filters)
    {
        foreach (var filter in filters)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, filter.PropertyName);
            var constant = Expression.Constant(filter.Value);
            var equal = Expression.Equal(property, constant);

            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
            query = query.Where(lambda);
        }

        return query;
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, List<SortCriterion> sorts)
    {
        bool isFirstSort = true;

        foreach (var sort in sorts)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sort.PropertyName);

            var lambda = Expression.Lambda(property, parameter);

            string methodName = isFirstSort ? (sort.SortDirection == SortDirection.Ascending ? "OrderBy" : "OrderByDescending")
                                            : (sort.SortDirection == SortDirection.Ascending ? "ThenBy" : "ThenByDescending");

            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { query.ElementType, property.Type },
                query.Expression,
                Expression.Quote(lambda)
            );

            query = query.Provider.CreateQuery<T>(methodCallExpression);
            isFirstSort = false;
        }

        return query;
    }
}

