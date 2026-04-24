using Serilog;
using System;
using System.Threading.Tasks;

namespace Dapper.Bowtie.Lambda
{
    public partial class LambdaQuery<T>
    {
        public int Count(int? timeout = null)
        {
            try
            {
                var statement = $"SELECT COUNT(*) {WhereStatement}";
                return Connection.QueryFirstOrDefault<int>(statement, Parameters, timeout);
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

        public async Task<int> CountAsync(int? timeout = null)
        {
            try
            {
                var statement = $"SELECT COUNT(*) {WhereStatement}";
                return await Connection.QueryFirstOrDefaultAsync<int>(statement, Parameters, timeout);
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