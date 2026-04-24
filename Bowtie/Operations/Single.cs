using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region single

        public T Single<T>(string statement, int? timeout = null)
        {
            return Single<T>(statement, null, timeout);
        }

        public T Single<T>(string statement, object param, int? timeout = null)
        {
            try
            {
                return QuerySingle<T>(statement, param, timeout);
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

        #region single async

        public async Task<T> SingleAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SingleAsync<T>(statement, null, timeout, cancellationToken);
        }

        public async Task<T> SingleAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await QuerySingleAsync<T>(statement, param, timeout, cancellationToken);
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

        public static T Single<T>(string statement, int? timeout = null)
        {
            return Default.Single<T>(statement, timeout);
        }

        public static T Single<T>(string statement, object param, int? timeout = null)
        {
            return Default.Single<T>(statement, param, timeout);
        }

        #endregion

        #region async

        public static async Task<T> SingleAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SingleAsync<T>(statement, timeout, cancellationToken);
        }

        public static async Task<T> SingleAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SingleAsync<T>(statement, param, timeout, cancellationToken);
        }

        #endregion
    }
}