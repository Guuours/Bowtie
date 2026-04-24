using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public T FirstOrDefault<T>(string statement, int? timeout = null)
        {
            return FirstOrDefault<T>(statement, null, timeout);
        }

        public T FirstOrDefault<T>(string statement, object param, int? timeout = null)
        {
            try
            {
                return QueryFirstOrDefault<T>(statement, param, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
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

        public async Task<T> FirstOrDefaultAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await FirstOrDefaultAsync<T>(statement, null, timeout, cancellationToken);
        }

        public async Task<T> FirstOrDefaultAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await QueryFirstOrDefaultAsync<T>(statement, param, timeout, cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return default;
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

        public static T FirstOrDefault<T>(string statement, int? timeout = null)
        {
            return Default.FirstOrDefault<T>(statement, timeout);
        }

        public static T FirstOrDefault<T>(string statement, object param, int? timeout = null)
        {
            return Default.FirstOrDefault<T>(statement, param, timeout);
        }

        #endregion

        #region async

        public static async Task<T> FirstOrDefaultAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.FirstOrDefaultAsync<T>(statement, timeout, cancellationToken);
        }

        public static async Task<T> FirstOrDefaultAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.FirstOrDefaultAsync<T>(statement, param, timeout, cancellationToken);
        }

        #endregion
    }
}