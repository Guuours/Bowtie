using System;
using System.Linq.Expressions;
using System.Text;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        internal LambdaQuery<T> Where(Expression exp)
        {
            var sb = new StringBuilder();
            ParseCondition(exp, sb, DatabaseType);
            WhereClause = $"WHERE {sb}";
            return this;
        }

        public LambdaQuery<T> Where(Expression<Func<T, bool>> exp)
        {
            return Where(exp.Body);
        }

        public LambdaQuery<T> Where<T1>(Expression<Func<T1, bool>> exp)
        {
            return Where(exp.Body);
        }

        public LambdaQuery<T> Where<T1, T2>(Expression<Func<T1, T2, bool>> exp)
        {
            return Where(exp.Body);
        }

        public LambdaQuery<T> Where<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> exp)
        {
            return Where(exp.Body);
        }

        public LambdaQuery<T> Where<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, bool>> exp)
        {
            return Where(exp.Body);
        }

        public LambdaQuery<T> Where<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, bool>> exp)
        {
            return Where(exp.Body);
        }

        public LambdaQuery<T> Where<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6, bool>> exp)
        {
            return Where(exp.Body);
        }

        public LambdaQuery<T> Where<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, bool>> exp)
        {
            return Where(exp.Body);
        }
    }
}