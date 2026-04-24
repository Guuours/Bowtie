using Dapper.Bowtie.Lambda;
using System;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        public LambdaQuery<T> From<T>(With? withHint = null)
        {
            // init lambda result
            var lambda = new LambdaQuery<T>
            {
                Connection = this
            };

            // prepare first join table
            var type = typeof(T);
            var tblName = SyntaxConstructor.GetTableName(type);
            var tblRef = new TableReference
            {
                DatabaseType = DatabaseType,
                JoinType = "FROM",
                Name = tblName,
                EntityType = type
            };
            lambda.TableRefs.Add(tblRef);

            // mssql with hint
            if (withHint.HasValue)
            {
                if (lambda.Connection.DatabaseType != DatabaseType.MSSQL && lambda.Connection.DatabaseType != DatabaseType.MSSQL_LEGACY)
                {
                    throw new Exception("Can't apply hint on MySQL or Oracle");
                }

                tblRef.WithHint = $"WITH ({withHint.Value})";
            }
            
            return lambda;
        }
    }

    public partial class DB
    {
        public static LambdaQuery<T> From<T>(With? withHint = null)
        {
            return Default.From<T>(withHint);
        }
    }
}

namespace Dapper.Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        public LambdaQuery<T> From<T1>()
        {
            // prepare first join table
            var type = typeof(T1);
            var tblName = SyntaxConstructor.GetTableName(type);
            var tblRef = new TableReference
            {
                DatabaseType = Connection.DatabaseType,
                JoinType = "FROM",
                Name = tblName,
                EntityType = type
            };
            TableRefs.Add(tblRef);

            return this;
        }
    }
}