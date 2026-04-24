using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public int Execute(string statement, object param, int? timeout = null)
        {
            try
            {
                return DoExecute(statement, param, timeout);
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

        public int Execute(string statement, int? timeout = null)
        {
            return Execute(statement, null, timeout);
        }

        #endregion

        #region async

        public async Task<int> ExecuteAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return await DoExecuteAsync(statement, param, timeout, cancellationToken);
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

        public async Task<int> ExecuteAsync(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(statement, null, timeout, cancellationToken);
        }

        #endregion
    }

    public partial class DB
    {
        #region sync

        public static int Execute(string statement, object param, int? timeout = null)
        {
            return Default.Execute(statement, param, timeout);
        }

        public static int Execute(string statement, int? timeout = null)
        {
            return Default.Execute(statement, timeout);
        }

        #endregion

        #region async

        public static async Task<int> ExecuteAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.ExecuteAsync(statement, param, timeout, cancellationToken);
        }

        public static async Task<int> ExecuteAsync(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.ExecuteAsync(statement, timeout, cancellationToken);
        }

        #endregion
    }
}