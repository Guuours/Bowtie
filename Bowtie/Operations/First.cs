using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region first

        public T First<T>(string statement, int? timeout = null)
        {
            return First<T>(statement, null, timeout);
        }

        public T First<T>(string statement, object param, int? timeout = null)
        {
            try
            {
                return QueryFirst<T>(statement, param, timeout);
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

        #region first async

        public async Task<T> FirstAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await FirstAsync<T>(statement, null, timeout, cancellationToken);
        }

        public async Task<T> FirstAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await QueryFirstAsync<T>(statement, param, timeout, cancellationToken);
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

        public static T First<T>(string statement, int? timeout = null)
        {
            return Default.First<T>(statement, timeout);
        }

        public static T First<T>(string statement, object param, int? timeout = null)
        {
            return Default.First<T>(statement, param, timeout);
        }

        #endregion

        #region async

        public static async Task<T> FirstAsync<T>(string statement, int? timeout = null)
        {
            return await Default.FirstAsync<T>(statement, timeout);
        }

        public static async Task<T> FirstAsync<T>(string statement, object param, int? timeout = null)
        {
            return await Default.FirstAsync<T>(statement, param, timeout);
        }

        #endregion
    }
}