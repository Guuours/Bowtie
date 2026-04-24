using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        #region single or default

        public T SingleOrDefault<T>(string statement, int? timeout = null)
        {
            return SingleOrDefault<T>(statement, null, timeout);
        }

        public T SingleOrDefault<T>(string statement, object param, int? timeout = null)
        {
            try
            {
                return QuerySingleOrDefault<T>(statement, param, timeout);
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

        #region single or default async

        public async Task<T> SingleOrDefaultAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SingleOrDefaultAsync<T>(statement, null, timeout, cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await SingleOrDefaultAsync<T>(statement, param, timeout, cancellationToken);
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

        public static T SingleOrDefault<T>(string statement, int? timeout = null)
        {
            return Default.SingleOrDefault<T>(statement, timeout);
        }

        public static T SingleOrDefault<T>(string statement, object param, int? timeout = null)
        {
            return Default.SingleOrDefault<T>(statement, param, timeout);
        }

        #endregion

        #region async

        public static async Task<T> SingleOrDefaultAsync<T>(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SingleOrDefaultAsync<T>(statement, timeout, cancellationToken);
        }

        public static async Task<T> SingleOrDefaultAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SingleOrDefaultAsync<T>(statement, param, timeout, cancellationToken);
        }

        #endregion
    }
}