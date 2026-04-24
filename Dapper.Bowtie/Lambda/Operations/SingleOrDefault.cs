using Serilog;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Dapper.Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        #region sync

        // simple/mapping query
        public T SingleOrDefault(int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(TableRefs[0].EntityType, Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return Connection.QuerySingleOrDefault<T>(statement, Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        // query with one param selector
        public T SingleOrDefault<T1>(Expression<Func<T1, T>> selector, int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(typeof(T1), Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return selector.Compile().Invoke(Connection.QuerySingleOrDefault<T1>(statement, Parameters, timeout));
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        #region query with multi param selector

        public T SingleOrDefault<T1, T2>(Expression<Func<T1, T2, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QuerySingleOrDefault(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public T SingleOrDefault<T1, T2, T3>(Expression<Func<T1, T2, T3, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QuerySingleOrDefault(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public T SingleOrDefault<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QuerySingleOrDefault(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public T SingleOrDefault<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QuerySingleOrDefault(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public T SingleOrDefault<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QuerySingleOrDefault(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public T SingleOrDefault<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QuerySingleOrDefault(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        #endregion

        #endregion

        #region async

        // simple/mapping query
        public async Task<T> SingleOrDefaultAsync(int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(TableRefs[0].EntityType, Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return await Connection.QuerySingleOrDefaultAsync<T>(statement, Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        // query with one param selector
        public async Task<T> SingleOrDefaultAsync<T1>(Expression<Func<T1, T>> selector, int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(typeof(T1), Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return selector.Compile().Invoke(await Connection.QuerySingleOrDefaultAsync<T1>(statement, Parameters, timeout));
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        #region query with multi param selector

        public async Task<T> SingleOrDefaultAsync<T1, T2>(Expression<Func<T1, T2, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QuerySingleOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<T> SingleOrDefaultAsync<T1, T2, T3>(Expression<Func<T1, T2, T3, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QuerySingleOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<T> SingleOrDefaultAsync<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QuerySingleOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<T> SingleOrDefaultAsync<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QuerySingleOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<T> SingleOrDefaultAsync<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QuerySingleOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<T> SingleOrDefaultAsync<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QuerySingleOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        #endregion

        #endregion
    }
}