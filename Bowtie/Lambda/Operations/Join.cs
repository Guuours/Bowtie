using System;
using System.Linq.Expressions;
using System.Text;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        internal LambdaQuery<T> Join<T1, T2>(Expression<Func<T1, T2, bool>> exp, string joinType)
        {
            // check if first table ref is already there
            if (TableRefs.Count == 0)
            {
                throw new Exception("Join must be used after From.");
            }

            // check if table alias is unique
            var tblRefFirst = TableRefs.Find(t => t.Alias == exp.Parameters[0].Name);
            var tblRefSecond = TableRefs.Find(t => t.Alias == exp.Parameters[1].Name);
            if (tblRefFirst != null && tblRefSecond != null)
            {
                throw new Exception("Table alias must be unique.");
            }

            // first join, assign alias for first table ref
            if (tblRefFirst == null && tblRefSecond == null)
            {
                var tblNameFirst = SyntaxConstructor.GetTableName(typeof(T1));
                var tblNameSecond = SyntaxConstructor.GetTableName(typeof(T2));
                var tblRefFrom = TableRefs[0];
                var firstAlias = string.Empty;
                if (tblNameFirst == tblRefFrom.Name)
                {
                    firstAlias = exp.Parameters[0].Name;
                    tblRefFirst = tblRefFrom;
                }
                if (tblNameSecond == tblRefFrom.Name)
                {
                    firstAlias = exp.Parameters[1].Name;
                    tblRefSecond = tblRefFrom;
                }
                tblRefFrom.Alias = firstAlias;
            }

            // add new table ref
            var tblRef = new TableReference
            {
                DatabaseType = Connection.DatabaseType,
                JoinType = joinType + " JOIN"
            };
            if (tblRefFirst == null)
            {
                var type = typeof(T1);
                tblRef.Name = SyntaxConstructor.GetTableName(type);
                tblRef.EntityType = type;
                tblRef.Alias = exp.Parameters[0].Name;
            }
            if (tblRefSecond == null)
            {
                var type = typeof(T2);
                tblRef.Name = SyntaxConstructor.GetTableName(type);
                tblRef.EntityType = type;
                tblRef.Alias = exp.Parameters[1].Name;
            }
            TableRefs.Add(tblRef);

            // parse on condition
            var sb = new StringBuilder();
            ParseCondition(exp.Body, sb, Connection.DatabaseType);
            tblRef.OnCondition = $"ON {sb}";

            return this;
        }

        public LambdaQuery<T> Join<T1, T2>(Expression<Func<T1, T2, bool>> exp)
        {
            return Join(exp, "INNER");
        }

        public LambdaQuery<T> LeftJoin<T1, T2>(Expression<Func<T1, T2, bool>> exp)
        {
            return Join(exp, "LEFT");
        }

        public LambdaQuery<T> RightJoin<T1, T2>(Expression<Func<T1, T2, bool>> exp)
        {
            return Join(exp, "RIGHT");
        }
    }
}