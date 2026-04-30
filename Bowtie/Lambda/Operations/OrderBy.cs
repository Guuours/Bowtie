using System;
using System.Linq.Expressions;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        public LambdaQuery<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            SortSpecs.Add(ParseSortSpec(keySelector.Body, true, DatabaseType));
            return this;
        }

        public LambdaQuery<T> OrderBy<T1>(Expression<Func<T1, object>> keySelector)
        {
            SortSpecs.Add(ParseSortSpec(keySelector.Body, true, DatabaseType));
            return this;
        }

        public LambdaQuery<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            SortSpecs.Add(ParseSortSpec(keySelector.Body, false, DatabaseType));
            return this;
        }

        public LambdaQuery<T> OrderByDescending<T1>(Expression<Func<T1, object>> keySelector)
        {
            SortSpecs.Add(ParseSortSpec(keySelector.Body, false, DatabaseType));
            return this;
        }
    }
}