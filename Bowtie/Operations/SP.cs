using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public List<T> SPQuery<T>(string spName, int? timeout = null)
        {
            return SPQuery<T>(spName, null, timeout);
        }

        public List<T> SPQuery<T>(string spName, object param, int? timeout = null)
        {
            try
            {
                // map type if not mapped yet
                TypeMapper.Map(typeof(T));

                // start stopwatch and log statement
                var stopwatch = Stopwatch.StartNew();
                Log.Debug($"Stored procedure: {spName}");

                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }

                // query
                var result = RawConnection.Query<T>(spName, param, Transaction, true, timeout, CommandType.StoredProcedure);
                var data = result.AsList() ?? new List<T>();

                // stop stopwatch and log result
                stopwatch.Stop();
                Log.Debug($"Query stored procedure in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recoreds found");

                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        public int SPExecute(string spName, int? timeout = null)
        {
            return SPExecute(spName, null, timeout);
        }

        public int SPExecute(string spName, object param, int? timeout = null)
        {
            try
            {
                // start stopwatch and log statement
                var stopwatch = Stopwatch.StartNew();
                Log.Debug($"Stored procedure: {spName}");

                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }

                // execute
                var affected = RawConnection.Execute(spName, param, Transaction, timeout, CommandType.StoredProcedure);

                // stop stopwatch and log result
                stopwatch.Stop();
                Log.Debug($"Stored procedure executed in: {stopwatch.ElapsedMilliseconds}ms, {affected} recored(s) affected");

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

        public async Task<List<T>> SPQueryAsync<T>(string spName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SPQueryAsync<T>(spName, null, timeout, cancellationToken);
        }

        public async Task<List<T>> SPQueryAsync<T>(string spName, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // map type if not mapped yet
                TypeMapper.Map(typeof(T));

                // start stopwatch and log statement
                var stopwatch = Stopwatch.StartNew();
                Log.Debug($"Stored procedure: {spName}");

                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }

                // query
                var result = await RawConnection.QueryAsync<T>(new CommandDefinition(spName, param, Transaction, timeout, CommandType.StoredProcedure, CommandFlags.Buffered, cancellationToken));
                var data = result.AsList() ?? new List<T>();

                // stop stopwatch and log result
                stopwatch.Stop();
                Log.Debug($"Query stored procedure in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recoreds found");

                return data;
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new List<T>();
            }
            finally
            {
                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        public async Task<int> SPExecuteAsync(string spName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await SPExecuteAsync(spName, null, timeout, cancellationToken);
        }

        public async Task<int> SPExecuteAsync(string spName, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // start stopwatch and log statement
                var stopwatch = Stopwatch.StartNew();
                Log.Debug($"Stored procedure: {spName}");

                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }

                // execute
                int affected = await RawConnection.ExecuteAsync(new CommandDefinition(spName, param, Transaction, timeout, CommandType.StoredProcedure, CommandFlags.Buffered, cancellationToken));

                // stop stopwatch and log result
                stopwatch.Stop();
                Log.Debug($"Stored procedure executed in: {stopwatch.ElapsedMilliseconds}ms, {affected} recored(s) affected");

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

        public static List<T> SPQuery<T>(string spName, int? timeout = null)
        {
            return Default.SPQuery<T>(spName, timeout);
        }

        public static List<T> SPQuery<T>(string spName, object param, int? timeout = null)
        {
            return Default.SPQuery<T>(spName, param, timeout);
        }

        public static int SPExecute(string spName, int? timeout = null)
        {
            return Default.SPExecute(spName, null, timeout);
        }

        public static int SPExecute(string spName, object param, int? timeout = null)
        {
            return Default.SPExecute(spName, param, timeout);
        }

        #endregion

        #region async

        public static async Task<List<T>> SPQueryAsync<T>(string spName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SPQueryAsync<T>(spName, null, timeout, cancellationToken);
        }

        public static async Task<List<T>> SPQueryAsync<T>(string spName, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SPQueryAsync<T>(spName, param, timeout, cancellationToken);
        }

        public static async Task<int> SPExecuteAsync(string spName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SPExecuteAsync(spName, null, timeout, cancellationToken);
        }

        public static async Task<int> SPExecuteAsync(string spName, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.SPExecuteAsync(spName, param, timeout, cancellationToken);
        }

        #endregion
    }
}