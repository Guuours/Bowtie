using Serilog;
using System;
using System.Threading.Tasks;

namespace Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        public int Update(int? timeout = null)
        {
            try
            {
                return Connection.Execute(UpdateStatement, Parameters, timeout);
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

        public async Task<int> UpdateAsync(int? timeout = null)
        {
            try
            {
                return await Connection.ExecuteAsync(UpdateStatement, Parameters, timeout);
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