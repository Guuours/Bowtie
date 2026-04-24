using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        internal List<T> Query<T>(string statement, object param, int? timeout = null)
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
            var result = RawConnection.Query<T>(statement, param, Transaction, true, timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal List<T> Query<T1, T2, T>(string statement, Func<T1, T2, T> selector, object param, int? timeout = null)
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
            var result = RawConnection.Query(statement, selector, param, Transaction, true, "Id", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal List<T> Query<T1, T2, T3, T>(string statement, Func<T1, T2, T3, T> selector, object param, int? timeout = null)
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
            var result = RawConnection.Query(statement, selector, param, Transaction, true, "ID", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal List<T> Query<T1, T2, T3, T4, T>(string statement, Func<T1, T2, T3, T4, T> selector, object param, int? timeout = null)
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
            var result = RawConnection.Query(statement, selector, param, Transaction, true, "ID", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal List<T> Query<T1, T2, T3, T4, T5, T>(string statement, Func<T1, T2, T3, T4, T5, T> selector, object param, int? timeout = null)
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
            var result = RawConnection.Query(statement, selector, param, Transaction, true, "ID", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal List<T> Query<T1, T2, T3, T4, T5, T6, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T> selector, object param, int? timeout = null)
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
            var result = RawConnection.Query(statement, selector, param, Transaction, true, "ID", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal List<T> Query<T1, T2, T3, T4, T5, T6, T7, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T7, T> selector, object param, int? timeout = null)
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
            var result = RawConnection.Query(statement, selector, param, Transaction, true, "ID", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        #endregion

        #region async

        internal async Task<List<T>> QueryAsync<T>(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
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
            var result = await RawConnection.QueryAsync<T>(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal async Task<List<T>> QueryAsync<T1, T2, T>(string statement, Func<T1, T2, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
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
            var result = await RawConnection.QueryAsync(statement, selector, param, Transaction, true, "Id", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal async Task<List<T>> QueryAsync<T1, T2, T3, T>(string statement, Func<T1, T2, T3, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
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
            var result = await RawConnection.QueryAsync(statement, selector, param, Transaction, true, "Id", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal async Task<List<T>> QueryAsync<T1, T2, T3, T4, T>(string statement, Func<T1, T2, T3, T4, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
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
            var result = await RawConnection.QueryAsync(statement, selector, param, Transaction, true, "Id", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal async Task<List<T>> QueryAsync<T1, T2, T3, T4, T5, T>(string statement, Func<T1, T2, T3, T4, T5, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
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
            var result = await RawConnection.QueryAsync(statement, selector, param, Transaction, true, "Id", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal async Task<List<T>> QueryAsync<T1, T2, T3, T4, T5, T6, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
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
            var result = await RawConnection.QueryAsync(statement, selector, param, Transaction, true, "Id", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        internal async Task<List<T>> QueryAsync<T1, T2, T3, T4, T5, T6, T7, T>(string statement, Func<T1, T2, T3, T4, T5, T6, T7, T> selector, object param, int? timeout = null, CancellationToken cancellationToken = default)
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
            var result = await RawConnection.QueryAsync(statement, selector, param, Transaction, true, "Id", timeout ?? DB.Config.DefaultTimeout, CommandType.Text);
            var data = result.AsList() ?? new List<T>();

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {data.Count} recored(s) found");

            return data;
        }

        #endregion
    }
}