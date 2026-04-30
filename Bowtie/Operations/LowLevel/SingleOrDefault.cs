using Dapper;
using Serilog;
using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        internal T QuerySingleOrDefault<T>(string statement, object param, int? timeout = null)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = RawConnection.QuerySingleOrDefault<T>(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal T QuerySingleOrDefault<T1, T2, T>(string statement, Func<T1, T2, T> selector, object param, int? timeout = null)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = RawConnection.QuerySingleOrDefault<T>(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal T QuerySingleOrDefault<T1, T2, T3, T>(string statement, Func<T1, T2, T3, T> selector, object param, int? timeout = null)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = RawConnection.QuerySingleOrDefault<T>(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal T QuerySingleOrDefault<T1, T2, T3, T4, T>(string statement, Func<T1, T2, T3, T4, T> selector, object param, int? timeout = null)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = RawConnection.QuerySingleOrDefault<T>(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal T QuerySingleOrDefault<T1, T2, T3, T4, T5, T>(string statement, Func<T1, T2, T3, T4, T5, T> selector, object param, int? timeout = null)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));
            TypeMapper.Map(typeof(T5));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = RawConnection.QuerySingleOrDefault<T>(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal T QuerySingleOrDefault<T1, T2, T3, T4, T5, T6, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T> selector, object param, int? timeout = null)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));
            TypeMapper.Map(typeof(T5));
            TypeMapper.Map(typeof(T6));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = RawConnection.QuerySingleOrDefault<T>(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal T QuerySingleOrDefault<T1, T2, T3, T4, T5, T6, T7, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T7, T> selector, object param, int? timeout = null)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));
            TypeMapper.Map(typeof(T5));
            TypeMapper.Map(typeof(T6));
            TypeMapper.Map(typeof(T7));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = RawConnection.QuerySingleOrDefault<T>(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        #endregion

        #region async

        internal async Task<T> QuerySingleOrDefaultAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = await RawConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal async Task<T> QuerySingleOrDefaultAsync<T1, T2, T>(string statement, Func<T1, T2, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = await RawConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal async Task<T> QuerySingleOrDefaultAsync<T1, T2, T3, T>(string statement, Func<T1, T2, T3, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = await RawConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal async Task<T> QuerySingleOrDefaultAsync<T1, T2, T3, T4, T>(string statement, Func<T1, T2, T3, T4, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = await RawConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal async Task<T> QuerySingleOrDefaultAsync<T1, T2, T3, T4, T5, T>(string statement, Func<T1, T2, T3, T4, T5, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));
            TypeMapper.Map(typeof(T5));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = await RawConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal async Task<T> QuerySingleOrDefaultAsync<T1, T2, T3, T4, T5, T6, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));
            TypeMapper.Map(typeof(T5));
            TypeMapper.Map(typeof(T6));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = await RawConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        internal async Task<T> QuerySingleOrDefaultAsync<T1, T2, T3, T4, T5, T6, T7, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T7, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // map type if not mapped yet
            TypeMapper.Map(typeof(T1));
            TypeMapper.Map(typeof(T2));
            TypeMapper.Map(typeof(T3));
            TypeMapper.Map(typeof(T4));
            TypeMapper.Map(typeof(T5));
            TypeMapper.Map(typeof(T6));
            TypeMapper.Map(typeof(T7));

            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // query
            var data = await RawConnection.QuerySingleOrDefaultAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultQueryTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms");

            return data;
        }

        #endregion
    }
}