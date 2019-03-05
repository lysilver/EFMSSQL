using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace ORM
{
    /// <summary>
    /// EF 扩展分页，在不知道具体字段的情况下，适合jpGrid、Datatable等js绑定table时使用
    /// </summary>
    public static class QueryableExtension
    {
        public static IOrderedQueryable<T> OrderByz<T>(this IQueryable<T> query, string propertyName)
        {
            return _OrderBy<T>(query, propertyName, false);
        }

        public static IOrderedQueryable<T> OrderByDescendingz<T>(this IQueryable<T> query, string propertyName)
        {
            return _OrderBy<T>(query, propertyName, true);
        }

        private static IOrderedQueryable<T> _OrderBy<T>(IQueryable<T> query, string propertyName, bool isDesc)
        {
            string methodname = (isDesc) ? "OrderByDescendingInternal" : "OrderByInternal";

            var memberProp = typeof(T).GetProperty(propertyName);

            var method = typeof(QueryableExtension).GetMethod(methodname)
                .MakeGenericMethod(typeof(T), memberProp.PropertyType);

            return (IOrderedQueryable<T>)method.Invoke(null, new object[] { query, memberProp });
        }

        public static IOrderedQueryable<T> OrderByInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {//public
            return query.OrderBy(_GetLamba<T, TProp>(memberProperty));
        }

        public static IOrderedQueryable<T> OrderByDescendingInternal<T, TProp>(IQueryable<T> query, PropertyInfo memberProperty)
        {//public
            return query.OrderByDescending(_GetLamba<T, TProp>(memberProperty));
        }

        private static Expression<Func<T, TProp>> _GetLamba<T, TProp>(PropertyInfo memberProperty)
        {
            if (memberProperty.PropertyType != typeof(TProp)) throw new Exception();

            var thisArg = Expression.Parameter(typeof(T));
            var lamba = Expression.Lambda<Func<T, TProp>>(Expression.Property(thisArg, memberProperty), thisArg);

            return lamba;
        }

        public static IQueryable<TSource> Between<TSource, TKey>
        (this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector,
            TKey low, TKey high) where TKey : IComparable<TKey>
        {
            Expression key = Expression.Invoke(keySelector,
                keySelector.Parameters.ToArray());
            Expression lowerBound = Expression.GreaterThanOrEqual
                (key, Expression.Constant(low));
            Expression upperBound = Expression.LessThanOrEqual
                (key, Expression.Constant(high));
            Expression and = Expression.AndAlso(lowerBound, upperBound);
            Expression<Func<TSource, bool>> lambda =
                Expression.Lambda<Func<TSource, bool>>(and, keySelector.Parameters);
            return source.Where(lambda);
        }
    }
}