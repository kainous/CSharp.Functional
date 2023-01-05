using System;
using System.Linq.Expressions;

namespace Maat.Functional {
    public static class ExpressionExtensions {
        public static Expression<bool> AndAlso(this Expression<bool> expression, Expression<bool> value) =>
            Expression.Lambda<bool>(Expression.AndAlso(expression, value));

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expression, Expression<Func<T, bool>> value) =>
            Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expression, value), expression.Parameters);

        //public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right) {
        //    return Expression.Lambda<TDelegate>(Expression.AndAlso(left, right), left.Parameters);
        //}
    }
}
