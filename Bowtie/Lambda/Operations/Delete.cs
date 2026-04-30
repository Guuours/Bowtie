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
                return Connection.Execute(DeleteStatement, Parameters, timeout);
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
                return await Connection.ExecuteAsync(DeleteStatement, Parameters, timeout);
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