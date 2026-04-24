using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public int Update(BaseEntity entry, int? timeout = null)
        {
            return Update(entry, null, timeout);
        }

        public int Update(BaseEntity entry, string tableName, int? timeout = null)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetUpdateStatement(entry.GetType(), tableName, DatabaseType);
                // execute
                var affected = DoExecute(statement, entry, timeout);
                // check affected
                if (affected > 1)
                {
                    Log.Warning("Multiple records have been affected.");
                }

                return affected;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return 0;
            }
            finally
            {
                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        public int Update<T>(object param, int? timeout = null)
        {
            return Update<T>(param, null, timeout);
        }

        public int Update<T>(object param, string tableName, int? timeout = null)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetUpdateStatement(typeof(T), tableName, DatabaseType);
                // execute
                var affected = DoExecute(statement, param, timeout);

                return affected;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return 0;
            }
            finally
            {
                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        #endregion

        #region async

        public async Task<int> UpdateAsync(BaseEntity entry, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync(entry, null, timeout, cancellationToken);
        }

        public async Task<int> UpdateAsync(BaseEntity entry, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetUpdateStatement(entry.GetType(), tableName, DatabaseType);
                // execute
                var affected = await DoExecuteAsync(statement, entry, timeout, cancellationToken);
                // check affected
                if (affected > 1)
                {
                    Log.Warning("Multiple records have been affected.");
                }

                return affected;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return 0;
            }
            finally
            {
                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        public async Task<int> UpdateAsync<T>(object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync<T>(param, null, timeout, cancellationToken);
        }

        public async Task<int> UpdateAsync<T>(object param, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetUpdateStatement(typeof(T), tableName, DatabaseType);
                // execute
                var affected = await DoExecuteAsync(statement, param, timeout, cancellationToken);

                return affected;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return 0;
            }
            finally
            {
                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        #endregion
    }

    public partial class DB
    {
        #region sync

        public static int Update(BaseEntity entry, int? timeout = null)
        {
            return Default.Update(entry, timeout);
        }

        public static int Update(BaseEntity entry, string tableName, int? timeout = null)
        {
            return Default.Update(entry, tableName, timeout);
        }

        public static int Update<T>(object param, int? timeout = null)
        {
            return Default.Update<T>(param, timeout);
        }

        public static int Update<T>(object param, string tableName, int? timeout = null)
        {
            return Default.Update<T>(param, tableName, timeout);
        }

        #endregion

        #region async

        public static async Task<int> UpdateAsync(BaseEntity entry, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.UpdateAsync(entry, timeout, cancellationToken);
        }

        public static async Task<int> UpdateAsync(BaseEntity entry, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.UpdateAsync(entry, tableName, timeout, cancellationToken);
        }

        public static async Task<int> UpdateAsync<T>(object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.UpdateAsync<T>(param, timeout, cancellationToken);
        }

        public static async Task<int> UpdateAsync<T>(object param, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.UpdateAsync<T>(param, tableName, timeout, cancellationToken);
        }

        #endregion
    }
}