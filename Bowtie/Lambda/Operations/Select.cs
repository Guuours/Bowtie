using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        #region sync

        // simple/mapping query
        public List<T> Select(int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(TableRefs[0].EntityType, Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return Connection.Query<T>(statement, Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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
        public List<T> Select<T1>(Expression<Func<T1, T>> selector, int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(typeof(T1), Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return Connection.Query<T1>(statement, Parameters, timeout).Select(selector.Compile()).AsList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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

        public List<T> Select<T1, T2>(Expression<Func<T1, T2, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.Query(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public List<T> Select<T1, T2, T3>(Expression<Func<T1, T2, T3, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.Query(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public List<T> Select<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.Query(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public List<T> Select<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.Query(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public List<T> Select<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.Query(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public List<T> Select<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return Connection.Query(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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
        public async Task<List<T>> SelectAsync(int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(TableRefs[0].EntityType, Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return await Connection.QueryAsync<T>(statement, Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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
        public async Task<List<T>> SelectAsync<T1>(Expression<Func<T1, T>> selector, int? timeout = null)
        {
            try
            {
                var statement = string.Join(" ", "SELECT", SyntaxConstructor.GetSelectColumns(typeof(T1), Connection.DatabaseType), FromClause, WhereClause, OrderByClause);
                return Connection.Query<T1>(statement, Parameters, timeout).Select(selector.Compile()).AsList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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

        public async Task<List<T>> SelectAsync<T1, T2>(Expression<Func<T1, T2, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<List<T>> SelectAsync<T1, T2, T3>(Expression<Func<T1, T2, T3, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<List<T>> SelectAsync<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<List<T>> SelectAsync<T1, T2, T3, T4, T5>(Expression<Func<T1, T2, T3, T4, T5, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<List<T>> SelectAsync<T1, T2, T3, T4, T5, T6>(Expression<Func<T1, T2, T3, T4, T5, T6, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<List<T>> SelectAsync<T1, T2, T3, T4, T5, T6, T7>(Expression<Func<T1, T2, T3, T4, T5, T6, T7, T>> selector, int? timeout = null)
        {
            try
            {
                var selectColumns = string.Join(", ", selector.Parameters.Select(p => p.Name + ".*").ToArray());
                var statement = string.Join(" ", "SELECT", selectColumns, FromClause, WhereClause, OrderByClause);
                return await Connection.QueryAsync(statement, selector.Compile(), Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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