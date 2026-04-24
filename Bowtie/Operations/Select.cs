using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region select

        public List<T> Select<T>(string statement, int? timeout = null)
        {
            return Select<T>(statement, null, timeout);
        }

        public List<T> Select<T>(string statement, object param, int? timeout = null)
        {
            try
            {
                return Query<T>(statement, param, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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

        #region select async

        public async Task<List<T>> SelectAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SelectAsync<T>(statement, null, timeout, cancellationToken);
        }

        public async Task<List<T>> SelectAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await QueryAsync<T>(statement, param, timeout, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
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

        #region select to dt

        public DataTable Select(string statement, int? timeout = null)
        {
            return Select(statement, null, timeout);
        }

        public DataTable Select(string statement, object param, int? timeout = null)
        {
            try
            {
                using (var reader = DoExecuteReader(statement, param, timeout))
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new DataTable();
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

        #region select to dt async

        public async Task<DataTable> SelectAsync(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SelectAsync(statement, null, timeout, cancellationToken);
        }

        public async Task<DataTable> SelectAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var reader = await DoExecuteReaderAsync(statement, param, timeout, cancellationToken))
                {
                    var dataTable = new DataTable();
                    dataTable.Load(reader);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new DataTable();
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

        public static List<T> Select<T>(string statement, int? timeout = null)
        {
            return Default.Select<T>(statement, timeout);
        }

        public static List<T> Select<T>(string statement, object param, int? timeout = null)
        {
            return Default.Select<T>(statement, param, timeout);
        }

        public static DataTable Select(string statement, int? timeout = null)
        {
            return Default.Select(statement, timeout);
        }

        public static DataTable Select(string statement, object param, int? timeout = null)
        {
            return Default.Select(statement, param, timeout);
        }

        #endregion

        #region async

        public static async Task<List<T>> SelectAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectAsync<T>(statement, timeout, cancellationToken);
        }

        public static async Task<List<T>> SelectAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectAsync<T>(statement, param, timeout, cancellationToken);
        }

        public static async Task<DataTable> SelectAsync(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectAsync(statement, timeout, cancellationToken);
        }

        public static async Task<DataTable> SelectAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectAsync(statement, param, timeout, cancellationToken);
        }

        #endregion
    }
}