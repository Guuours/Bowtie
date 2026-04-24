using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        public List<T> SelectPage(int step, int size, int? timeout = null)
        {
            try
            {
                var statement = $"SELECT {SyntaxConstructor.GetSelectColumns(typeof(T), Connection.DatabaseType)} {WhereStatementWithOrderBy}";
                statement = SyntaxConstructor.GetSelectPageStatement(step, size, statement, Connection.DatabaseType);
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

        public async Task<List<T>> SelectPageAsync(int step, int size, int? timeout = null)
        {
            try
            {
                var statement = $"SELECT {SyntaxConstructor.GetSelectColumns(typeof(T), Connection.DatabaseType)} {WhereStatementWithOrderBy}";
                statement = SyntaxConstructor.GetSelectPageStatement(step, size, statement, Connection.DatabaseType);
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

        public List<T> SelectPage<T>(int step, int size, int? timeout = null)
        {
            try
            {
                var statement = $"SELECT * {WhereStatementWithOrderBy}";
                statement = SyntaxConstructor.GetSelectPageStatement(step, size, statement, Connection.DatabaseType);
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

        public async Task<List<T>> SelectPageAsync<T>(int step, int size, int? timeout = null)
        {
            try
            {
                var statement = $"SELECT * {WhereStatementWithOrderBy}";
                statement = SyntaxConstructor.GetSelectPageStatement(step, size, statement, Connection.DatabaseType);
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

        //public List<T> SelectPage<T>(Func<T, T> selector, int step, int size, int? timeout = null)
        //{
        //    return SelectPage(step, size, timeout).Select(selector).AsList();
        //}

        //public async Task<List<T>> SelectPageAsync<T>(Func<T, T> selector, int step, int size, int? timeout = null)
        //{
        //    return (await SelectPageAsync(step, size, timeout)).Select(selector).AsList();
        //}
    }
}