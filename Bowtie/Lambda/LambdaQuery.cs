using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bowtie.Lambda
{
    internal class TableReference
    {
        public DatabaseType DatabaseType { get; set; }

        public string JoinType { get; set; }

        public string Name { get; set; }

        public Type EntityType { get; set; }

        public string Alias { get; set; }

        public string WithHint { get; set; }

        public string OnCondition { get; set; }

        public string ClauseStatement
        {
            get
            {
                var list = new List<string> { JoinType, Name.ApplyTableModifier(DatabaseType), Alias };
                if (!string.IsNullOrEmpty(WithHint))
                {
                    list.Add(WithHint);
                }
                if (!string.IsNullOrEmpty(OnCondition))
                {
                    list.Add(OnCondition);
                }
                return string.Join(" ", list);
            }
        }
    }

    public partial class LambdaQuery<T>
    {
        public DatabaseType DatabaseType { get; set; }

        internal Connection Connection { get; set; }

        internal List<string> Assignments { get; set; } = new List<string>();

        internal string SetClause
        {
            get
            {
                if (Assignments.Count > 0)
                {
                    return "SET " + string.Join(", ", Assignments);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        internal List<TableReference> TableRefs { get; set; } = new List<TableReference>();

        internal string FromClause
        {
            get
            {
                return string.Join(" ", TableRefs.Select(t => t.ClauseStatement));
            }
        }

        internal string WhereClause { get; set; }

        internal List<string> SortSpecs { get; set; } = new List<string>();

        internal string OrderByClause
        {
            get
            {
                if (SortSpecs.Count > 0)
                {
                    return "ORDER BY " + string.Join(", ", SortSpecs);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string UpdateStatement
        {
            get
            {
                if (TableRefs.Count < 2)
                {
                    return string.Join(" ", "UPDATE", TableRefs.First().Name.ApplyTableModifier(DatabaseType), SetClause, WhereClause);
                }
                else
                {
                    return string.Format(SyntaxAdapter.GetJoinUpdateStatement(DatabaseType), TableRefs.First().Alias, SetClause, FromClause.Substring(5), WhereClause);
                }
            }
        }

        public string DeleteStatement
        {
            get
            {
                if (TableRefs.Count < 2)
                {
                    return string.Join(" ", "DELETE", FromClause, WhereClause);
                }
                else
                {
                    return string.Join(" ", "DELETE", TableRefs.First().Alias, FromClause, WhereClause);
                }
            }
        }

        internal DynamicParameters Parameters { get; set; } = new DynamicParameters();

        private int ParametersIndex { get; set; } = 0;

        internal string Parameterize(object param)
        {
            var name = "param" + ++ParametersIndex;
            if (param is bool)
            {
                Parameters.Add(name, (bool)param ? 1 : 0);
            }
            else
            {
                Parameters.Add(name, param);
            }
            return SyntaxAdapter.ApplyParameterPrefix(name, DatabaseType);
        }
    }
}