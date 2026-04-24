using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public int Truncate<T>(int? timeout = null)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetTruncateStatement(typeof(T), DatabaseType);
                // execute
                var affected = DoExecute(statement, null, timeout);

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

        public async Task<int> TruncateAsync<T>(int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetTruncateStatement(typeof(T), DatabaseType);
                // execute
                var affected = await DoExecuteAsync(statement, null, timeout, cancellationToken);

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

        public int Truncate<T>(int? timeout = null)
        {
            return Default.Truncate<T>(timeout);
        }

        #endregion

        #region async

        public static async Task<int> TruncateAsync<T>(int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.TruncateAsync<T>(timeout, cancellationToken);
        }

        #endregion
    }
}