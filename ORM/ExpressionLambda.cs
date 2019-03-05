using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class ExpressionLambda
    {
        public static Expression GetExpression<T>(Dictionary<string, object> dict) where T : class, new()
        {
            var type = typeof(T);
            var parameter = Expression.Parameter(type);
            Expression expr = Expression.Constant(true);
            var methodInfo = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
            foreach (var item in dict)
            {
                if (!string.IsNullOrEmpty(item.Value.ToString()))
                {
                    expr = Expression.And(expr,
                        Expression.Equal(Expression.Property(parameter, item.Key), Expression.Constant(item.Value.ToString())));
                }
            }
            var lambda = Expression.Lambda<Func<T, bool>>(expr, parameter);
            return lambda;
        }
    }
}