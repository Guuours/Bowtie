using Dapper.Bowtie.Lambda;
using Serilog;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Dapper.Bowtie
{
    internal static class SyntaxConstructor
    {
        private static Regex orderbyPattern = new Regex("\\s+(?i)ORDER\\s+BY(?-i)[\\S\\s]+$");

        internal static (string statement, string orderBy) SplitClause(string statement)
        {
            var lastFromIndex = statement.LastIndexOf("FROM", StringComparison.InvariantCultureIgnoreCase);
            var match = orderbyPattern.Match(statement, lastFromIndex);
            if (match.Success)
            {
                return (statement.Replace(match.Value, ""), match.Value.Trim());
            }

            return (statement, null);
        }

        private static string GetCachedStatement(string key)
        {
            return Cache.Statements.ContainsKey(key) ? Cache.Statements[key] : null;
        }

        internal static string GetTableName(Type type)
        {
            var tblAttr = type.GetCustomAttribute<TableAttribute>(false);
            return tblAttr == null ? type.Name : (string.IsNullOrEmpty(tblAttr.Name) ? type.Name : tblAttr.Name);
        }

        internal static string GetColumnName(PropertyInfo prop)
        {
            var colAttr = prop.GetCustomAttribute<ColumnAttribute>(false);
            return colAttr == null ? prop.Name : (string.IsNullOrEmpty(colAttr.Name) ? prop.Name : colAttr.Name);
        }

        internal static string GetSelectColumns(Type type, DatabaseType dbType)
        {
            var cacheKey = string.Join("_", "SELECT_COLUMNS", dbType, type.GUID.ToString("N"));
            var statement = GetCachedStatement(cacheKey);

            if (statement == null)
            {
                List<string> columns = new List<string>();
                foreach (var prop in type.GetProperties())
                {
                    var colAttr = prop.GetCustomAttribute<ColumnAttribute>(false);
                    if (colAttr != null)
                    {
                        if (!colAttr.Ignore.HasFlag(When.Select))
                        {
                            columns.Add((string.IsNullOrEmpty(colAttr.Name) ? prop.Name : colAttr.Name).ApplyColumnModifier(dbType));
                        }
                    }
                }
                statement = string.Join(", ", columns);

                Cache.Statements.TryAdd(cacheKey, statement);
            }

            return statement;
        }

        internal static string GetSelectColumns(Type type, string alias, DatabaseType dbType)
        {
            var cacheKey = string.Join("_", "SELECT_COLUMNS", dbType, type.GUID.ToString("N"), alias);
            var statement = GetCachedStatement(cacheKey);

            if (statement == null)
            {
                List<string> columns = new List<string>();
                foreach (var prop in type.GetProperties())
                {
                    var colAttr = prop.GetCustomAttribute<ColumnAttribute>(false);
                    if (colAttr != null)
                    {
                        if (!colAttr.Ignore.HasFlag(When.Select))
                        {
                            columns.Add(alias + "." + (string.IsNullOrEmpty(colAttr.Name) ? prop.Name : colAttr.Name).ApplyColumnModifier(dbType));
                        }
                    }
                }
                statement = string.Join(", ", columns);

                Cache.Statements.TryAdd(cacheKey, statement);
            }

            return statement;
        }

        internal static string GetSelectColumns(List<TableReference> tableRefs, DatabaseType dbType, params string[] aliases)
        {
            var columnGroups = new List<string>();
            foreach (var alias in aliases)
            {
                var tableRef = tableRefs.Find(t => t.Alias == alias);
                if (tableRef != null)
                {
                    columnGroups.Add(GetSelectColumns(tableRef.EntityType, alias, dbType));
                }
            }
            return string.Join(", ", columnGroups);
        }

        internal static string GetCountStatement(string statement, DatabaseType dbType)
        {
            var result = SplitClause(statement);
            statement = SyntaxAdapter.GetCountStatement(dbType, result.statement);

            return statement;
        }

        internal static string GetSelectPageStatement(int step, int size, string statement, DatabaseType dbType)
        {
            var result = SplitClause(statement);
            if (string.IsNullOrEmpty(result.orderBy))
            {
                result.orderBy = SyntaxAdapter.GetDefaultOrderBy(dbType);
            }
            statement = SyntaxAdapter.GetPaginationStatement(dbType, result.statement, result.orderBy);

            // calculate pagination
            statement = string.Format(statement, (step - 1) * size, size);

            return statement;
        }

        internal static string GetInsertStatement(Type type, string tableName, DatabaseType dbType)
        {
            var cacheKey = string.Join("_", "INSERT", dbType,
                type == null ? Guid.Empty.ToString("N") : type.GUID.ToString("N"),
                string.IsNullOrEmpty(tableName) ? "N/A" : tableName);
            var statement = GetCachedStatement(cacheKey);

            if (statement == null)
            {
                // get table name from type
                if (string.IsNullOrEmpty(tableName) && type != null)
                {
                    tableName = GetTableName(type);
                }

                // get column names and parameter names
                var columns = new List<string>();
                var parameters = new List<string>();
                foreach (var prop in type.GetProperties())
                {
                    var colAttr = prop.GetCustomAttribute<ColumnAttribute>(false);
                    if (colAttr != null)
                    {
                        // if it's ignored, skip it
                        if (!colAttr.Ignore.HasFlag(When.Insert))
                        {
                            columns.Add((string.IsNullOrEmpty(colAttr.Name) ? prop.Name : colAttr.Name).ApplyColumnModifier(dbType));
                            parameters.Add(prop.Name.ApplyParameterPrefix(dbType));
                        }
                    }
                    else
                    {
                        columns.Add(prop.Name.ApplyColumnModifier(dbType));
                        parameters.Add(prop.Name.ApplyParameterPrefix(dbType));
                    }
                }

                // construct statement
                statement = SyntaxAdapter.GetInsertStatement(dbType);
                statement = string.Format(statement, tableName.ApplyTableModifier(dbType), string.Join(", ", columns), string.Join(", ", parameters));
                Cache.Statements.TryAdd(cacheKey, statement);
            }

            return statement;
        }

        internal static string GetUpdateStatement(Type type, string tableName, DatabaseType dbType)
        {
            var cacheKey = string.Join("_", "UPDATE", dbType,
                type == null ? Guid.Empty.ToString("N") : type.GUID.ToString("N"),
                string.IsNullOrEmpty(tableName) ? "N/A" : tableName);
            var statement = GetCachedStatement(cacheKey);

            if (statement == null)
            {
                // get table name from type
                if (string.IsNullOrEmpty(tableName) && type != null)
                {
                    tableName = GetTableName(type);
                }

                // get assignments and conditions
                var assignments = new List<string>();
                var conditions = new List<string>();
                foreach (var prop in type.GetProperties())
                {
                    var colAttr = prop.GetCustomAttribute<ColumnAttribute>(false);
                    if (colAttr != null)
                    {
                        var colName = string.IsNullOrEmpty(colAttr.Name) ? prop.Name : colAttr.Name;
                        var assignment = string.Format("{0} = {1}", colName.ApplyColumnModifier(dbType), prop.Name.ApplyParameterPrefix(dbType));
                        // if it's ignored, skip it
                        if (!colAttr.Ignore.HasFlag(When.Update))
                        {
                            assignments.Add(assignment);
                        }
                        // if it's primary key, use it as condition
                        if (colAttr.PK)
                        {
                            conditions.Add(assignment);
                        }
                    }
                }

                // check conditions
                if (conditions.Count == 0)
                {
                    Log.Warning("Warning: This operation would affect all records in table, are you missing where clause?");
                }

                // construct statement
                statement = string.Format("UPDATE {0}", tableName.ApplyTableModifier(dbType));
                statement = string.Format("{0} SET {1} WHERE {2}", statement, string.Join(", ", assignments), string.Join(" AND ", conditions));
                Cache.Statements.TryAdd(cacheKey, statement);
            }

            return statement;
        }

        internal static string GetDeleteStatement(Type type, DatabaseType dbType)
        {
            var cacheKey = string.Join("_", "DELETE", dbType, type.GUID.ToString("N"));
            var statement = GetCachedStatement(cacheKey);

            if (statement == null)
            {
                // get table name from type
                var tableName = GetTableName(type);
                statement = string.Format("DELETE FROM {0}", tableName.ApplyTableModifier(dbType));

                var conditions = new List<string>();
                foreach (var prop in type.GetProperties())
                {
                    var colAttr = prop.GetCustomAttribute<ColumnAttribute>(false);
                    if (colAttr != null)
                    {
                        // get column name
                        var colName = string.IsNullOrEmpty(colAttr.Name) ? prop.Name : colAttr.Name;
                        // if it's primary key, use it as condition
                        if (colAttr.PK)
                        {
                            conditions.Add(string.Format("{0} = {1}", colName.ApplyColumnModifier(dbType), prop.Name.ApplyParameterPrefix(dbType)));
                        }
                    }
                }

                // check conditions
                if (conditions.Count == 0)
                {
                    Log.Warning("Warning: This operation would affect all records in table, are you missing where clause?");
                }

                statement = string.Format("{0} WHERE {1}", statement, string.Join(" AND ", conditions));
                Cache.Statements.TryAdd(cacheKey, statement);
            }

            return statement;
        }

        internal static string GetTruncateStatement(Type type, DatabaseType dbType)
        {
            var cacheKey = string.Join("_", "TRUNCATE", dbType, type.GUID.ToString("N"));
            var statement = GetCachedStatement(cacheKey);

            if (statement == null)
            {
                var tableName = GetTableName(type);
                statement = string.Format("TRUNCATE TABLE {0}", tableName.ApplyTableModifier(dbType));

                Cache.Statements.TryAdd(cacheKey, statement);
            }

            return statement;
        }
    }
}