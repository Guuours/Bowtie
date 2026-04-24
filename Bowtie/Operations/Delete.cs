using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public int Delete(BaseEntity entry, int? timeout = null)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetDeleteStatement(entry.GetType(), DatabaseType);
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

        #endregion

        #region async

        public async Task<int> DeleteAsync(BaseEntity entry, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetDeleteStatement(entry.GetType(), DatabaseType);
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

        #endregion
    }

    public partial class DB
    {
        #region sync

        public static int Delete(BaseEntity entry, int? timeout = null)
        {
            return Default.Delete(entry, timeout);
        }

        #endregion

        #region async

        public static async Task<int> DeleteAsync(BaseEntity entry, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.DeleteAsync(entry, timeout, cancellationToken);
        }

        #endregion
    }
}