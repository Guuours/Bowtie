using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        #region select page

        public List<T> SelectPage<T>(int step, int size, string statement, int? timeout = null)
        {
            return SelectPage<T>(step, size, statement, null, timeout);
        }

        public List<T> SelectPage<T>(int step, int size, string statement, object param, int? timeout = null)
        {
            if (step <= 0 || size <= 0)
            {
                return Select<T>(statement, param, timeout);
            }

            return Select<T>(SyntaxConstructor.GetSelectPageStatement(step, size, statement, DatabaseType), param, timeout);
        }

        #endregion

        #region select page async

        public async Task<List<T>> SelectPageAsync<T>(int step, int size, string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SelectPageAsync<T>(step, size, statement, null, timeout, cancellationToken);
        }

        public async Task<List<T>> SelectPageAsync<T>(int step, int size, string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            if (step <= 0 || size <= 0)
            {
                return await SelectAsync<T>(statement, param, timeout, cancellationToken);
            }

            return await SelectAsync<T>(SyntaxConstructor.GetSelectPageStatement(step, size, statement, DatabaseType), param, timeout, cancellationToken);
        }

        #endregion

        #region select page to dt

        public DataTable SelectPage(int step, int size, string statement, int? timeout = null)
        {
            return SelectPage(step, size, statement, null, timeout);
        }

        public DataTable SelectPage(int step, int size, string statement, object param, int? timeout = null)
        {
            if (step <= 0 || size <= 0)
            {
                return Select(statement, param, timeout);
            }

            return Select(SyntaxConstructor.GetSelectPageStatement(step, size, statement, DatabaseType), param, timeout);
        }

        #endregion

        #region select page to dt async

        public async Task<DataTable> SelectPageAsync(int step, int size, string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SelectPageAsync(step, size, statement, null, timeout, cancellationToken);
        }

        public async Task<DataTable> SelectPageAsync(int step, int size, string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            if (step <= 0 || size <= 0)
            {
                return await SelectAsync(statement, param, timeout, cancellationToken);
            }

            return await SelectAsync(SyntaxConstructor.GetSelectPageStatement(step, size, statement, DatabaseType), param, timeout, cancellationToken);
        }

        #endregion
    }

    public partial class DB
    {
        #region sync

        public static List<T> SelectPage<T>(int step, int size, string statement, int? timeout = null)
        {
            return Default.SelectPage<T>(step, size, statement, timeout);
        }

        public static List<T> SelectPage<T>(int step, int size, string statement, object param, int? timeout = null)
        {
            return Default.SelectPage<T>(step, size, statement, param, timeout);
        }

        public static DataTable SelectPage(int step, int size, string statement, int? timeout = null)
        {
            return Default.SelectPage(step, size, statement, timeout);
        }

        public static DataTable SelectPage(int step, int size, string statement, object param, int? timeout = null)
        {
            return Default.SelectPage(step, size, statement, param, timeout);
        }

        #endregion

        #region async

        public static async Task<List<T>> SelectPageAsync<T>(int step, int size, string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectPageAsync<T>(step, size, statement, timeout, cancellationToken);
        }

        public static async Task<List<T>> SelectPageAsync<T>(int step, int size, string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectPageAsync<T>(step, size, statement, param, timeout, cancellationToken);
        }

        public static async Task<DataTable> SelectPageAsync(int step, int size, string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectPageAsync(step, size, statement, timeout, cancellationToken);
        }

        public static async Task<DataTable> SelectPageAsync(int step, int size, string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SelectPageAsync(step, size, statement, param, timeout, cancellationToken);
        }

        #endregion
    }
}