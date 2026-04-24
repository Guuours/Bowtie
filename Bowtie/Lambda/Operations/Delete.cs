using Serilog;
using System;
using System.Threading.Tasks;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        public int Delete(int? timeout = null)
        {
            try
            {
                var statement = $"DELETE {WhereStatement}";
                return Connection.Execute(statement, Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return 0;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }

        public async Task<int> DeleteAsync(int? timeout = null)
        {
            try
            {
                var statement = $"DELETE {WhereStatement}";
                return await Connection.ExecuteAsync(statement, Parameters, timeout);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return 0;
            }
            finally
            {
                if (Connection.AutoRelease)
                {
                    Connection.Dispose();
                }
            }
        }
    }
}