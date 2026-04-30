using Serilog;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        #region sync

        // simple/mapping query
        public T FirstOrDefault(int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(TableRefs.First().EntityType, DatabaseType), FromClause, WhereClause, OrderByClause);
                return Connection.QueryFirstOrDefault<T>(statement, Parameters, timeout);
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
        public T FirstOrDefault<T1>(Expression<Func<T1, T>> selector, int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(typeof(T1), DatabaseType), FromClause, WhereClause, OrderByClause);
                return selector.Compile().Invoke(Connection.QueryFirstOrDefault<T1>(statement, Parameters, timeout));
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

        public T FirstOrDefault<T1, T2>(Expression<Func<T1, T2, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QueryFirstOrDefault(statement, selector.Compile(), Parameters, timeout);
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

        public T FirstOrDefault<T1, T2, T3>(Expression<Func<T1, T2, T3, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QueryFirstOrDefault(statement, selector.Compile(), Parameters, timeout);
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

        public T FirstOrDefault<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QueryFirstOrDefault(statement, selector.Compile(), Parameters, timeout);
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

        public T FirstOrDefault<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QueryFirstOrDefault(statement, selector.Compile(), Parameters, timeout);
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

        public T FirstOrDefault<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QueryFirstOrDefault(statement, selector.Compile(), Parameters, timeout);
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

        public T FirstOrDefault<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.QueryFirstOrDefault(statement, selector.Compile(), Parameters, timeout);
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
        public async Task<T> FirstOrDefaultAsync(int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(TableRefs.First().EntityType, DatabaseType), FromClause, WhereClause, OrderByClause);
                return await Connection.QueryFirstOrDefaultAsync<T>(statement, Parameters, timeout);
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
        public async Task<T> FirstOrDefaultAsync<T1>(Expression<Func<T1, T>> selector, int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(typeof(T1), DatabaseType), FromClause, WhereClause, OrderByClause);
                return selector.Compile().Invoke(await Connection.QueryFirstOrDefaultAsync<T1>(statement, Parameters, timeout));
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

        public async Task<T> FirstOrDefaultAsync<T1, T2>(Expression<Func<T1, T2, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryFirstOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
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

        public async Task<T> FirstOrDefaultAsync<T1, T2, T3>(Expression<Func<T1, T2, T3, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryFirstOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
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

        public async Task<T> FirstOrDefaultAsync<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryFirstOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
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

        public async Task<T> FirstOrDefaultAsync<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryFirstOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
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

        public async Task<T> FirstOrDefaultAsync<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryFirstOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
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

        public async Task<T> FirstOrDefaultAsync<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryFirstOrDefaultAsync(statement, selector.Compile(), Parameters, timeout);
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