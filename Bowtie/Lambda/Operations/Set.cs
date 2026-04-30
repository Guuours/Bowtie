using System;
using System.Linq.Expressions;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        public LambdaQuery<T> Set(Expression<Func<T, bool>> exp)
        {
            ParseAssignment(exp.Body, Assignments, DatabaseType);
            return this;
        }
    }
}