namespace Bowtie
{
    public static class SyntaxAdapter
    {
        public static string ApplyParameterPrefix(this string paramName, DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL:
                case DatabaseType.MSSQL_LEGACY:
                    return "@" + paramName;
                //case DatabaseType.Oracle:
                //    return ":" + paramName;
                case DatabaseType.MYSQL:
                    return "@" + paramName;
                default:
                    return paramName;
            }
        }

        public static string ApplyTableModifier(this string tableName, DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL:
                case DatabaseType.MSSQL_LEGACY:
                    return "[" + tableName + "]";
                //case DatabaseType.Oracle:
                //    return tableName;
                case DatabaseType.MYSQL:
                    return "`" + tableName + "`";
                default:
                    return tableName;
            }
        }

        public static string ApplyColumnModifier(this string columnName, DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL:
                case DatabaseType.MSSQL_LEGACY:
                    return "[" + columnName + "]";
                //case DatabaseType.Oracle:
                //    return columnName;
                case DatabaseType.MYSQL:
                    return "`" + columnName + "`";
                default:
                    return columnName;
            }
        }

        internal static string GetInsertStatement(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL:
                case DatabaseType.MSSQL_LEGACY:
                    return "INSERT INTO {0}({1}) VALUES ({2}); SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";
                //case DatabaseType.Oracle:
                //    return string.Format("INSERT INTO {{0}}({{1}}) VALUES ({{2}}) RETURNING {0} INTO :Inserted", identity.ApplyColumnModifier(type));
                case DatabaseType.MYSQL:
                    return "INSERT INTO {0}({1}) VALUES ({2}); SELECT LAST_INSERT_ID();";
                default:
                    return string.Empty;
            }
        }

        internal static string GetCountStatement(DatabaseType type, string statement)
        {
            switch (type)
            {
                case DatabaseType.MSSQL:
                case DatabaseType.MSSQL_LEGACY:
                    return string.Format("SELECT COUNT(*) FROM ({0}) AS _INNER", statement);
                //case DatabaseType.Oracle:
                //    return string.Format("SELECT COUNT(*) FROM ({0}) INNER", statement);
                case DatabaseType.MYSQL:
                    return string.Format("SELECT COUNT(*) FROM ({0}) _INNER", statement);
                default:
                    return string.Empty;
            }
        }

        internal static string GetPaginationStatement(DatabaseType type, string statement, string orderby)
        {
            if (string.IsNullOrEmpty(orderby))
            {
                orderby = GetDefaultOrderBy(type);
            }

            switch (type)
            {
                case DatabaseType.MSSQL_LEGACY:
                    return string.Format(@"
SELECT TOP ({{1}}) * FROM
(
    SELECT INNER_QUERY.*, ROW_NUMBER() OVER ({1}) AS ROWNUM FROM
    (
        {0}
    ) AS INNER_QUERY
) AS OUTER_QUERY WHERE ROWNUM > {{0}} ORDER BY ROWNUM", statement, orderby);
                case DatabaseType.MSSQL:
                    return string.Format(@"
{0} {1} OFFSET {{0}} ROWS FETCH NEXT {{1}} ROWS ONLY", statement, orderby);
//                case DatabaseType.Oracle:
//                    return string.Format(@"
//SELECT * FROM
//(
//    SELECT INNER_QUERY.*, ROW_NUMBER() OVER ({1}) INNER_ROWNUM FROM
//    (
//        {0}
//    ) INNER_QUERY
//) OUTER_QUERY WHERE INNER_ROWNUM > {{0}} AND ROWNUM <= {{1}}", statement, orderby);
                case DatabaseType.MYSQL:
                    return string.Format("{0} {1} LIMIT {{0}}, {{1}}", statement, orderby);
                default:
                    return string.Empty;
            }
        }

        internal static string GetDefaultOrderBy(DatabaseType type)
        {
            switch (type)
            {
                case DatabaseType.MSSQL:
                case DatabaseType.MSSQL_LEGACY:
                    return "ORDER BY (SELECT 0)";
                //case DatabaseType.Oracle:
                case DatabaseType.MYSQL:
                    return "ORDER BY 1";
                default:
                    return string.Empty;
            }
        }
    }
}