using Dapper.Bowtie.Lambda;
using System;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        public LambdaQuery<T> Query<T>()
        {
            // init lambda result
            var lambda = new LambdaQuery<T>
            {
                Connection = this
            };

            return lambda;
        }
    }

    public partial class DB
    {
        public static LambdaQuery<T> Query<T>()
        {
            return Default.Query<T>();
        }
    }
}