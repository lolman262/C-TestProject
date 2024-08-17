using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace DynamicQueryLibrary
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IEnumerable<FilterCriteria> filterCriterias)
        {
            if (filterCriterias == null || !filterCriterias.Any())
            {
                return source;
            }

            // Create a parameter expression
            var parameter = Expression.Parameter(typeof(T), "x");
            // Create a binary expression
            var expression = filterCriterias
            .Select(criteria => GetExpression<T>(parameter, criteria))
            .Aggregate(Expression.AndAlso);
            var predicate = Expression.Lambda<Func<T, bool>>(expression, parameter);
            return source.Where(predicate);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IEnumerable<SortCriteria> sortCriterias)
        {
            if (sortCriterias == null || !sortCriterias.Any())
            {
                return source;
            }
            var parameter = Expression.Parameter(typeof(T), "x");
            var orderedQuery = source;
            foreach (var criteria in sortCriterias)
            {
                var property = Expression.Property(parameter, criteria.PropertyName);
                var lambda = Expression.Lambda(property, parameter);
                var methodName = criteria.SortDirection == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
                var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { typeof(T), property.Type },
                orderedQuery.Expression,
                Expression.Quote(lambda));
                orderedQuery = orderedQuery.Provider.CreateQuery<T>(resultExpression);
            }
            return orderedQuery;
        }
        private static Expression GetExpression<T>(ParameterExpression parameter, FilterCriteria criteria)
        {
            var property = Expression.Property(parameter, criteria.PropertyName);
            var constant = Expression.Constant(criteria.Value);
            return Expression.Equal(property, constant);
        }
    }
    public class FilterCriteria
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }
    public class SortCriteria
    {
        public string PropertyName { get; set; }
        public SortDirection SortDirection { get; set; }
    }
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}