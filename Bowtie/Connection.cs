using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public enum DatabaseType
    {
        MSSQL,
        MSSQL_LEGACY,
        MYSQL
    }

    public partial class Connection : IDisposable
    {
        public string Name { get; set; }

        public DatabaseType DatabaseType { get; set; }

        public DbConnection RawConnection { get; set; }

        internal IDbTransaction Transaction { get; set; }

        public Connection Use(string dbName)
        {
            RawConnection.ChangeDatabase(dbName);
            return this;
        }

        internal bool AutoRelease { get; set; } = true;

        public Connection KeepAlive()
        {
            AutoRelease = false;

            // register context conn
            var threadId = Thread.CurrentThread.ManagedThreadId;
            if (!DB.ContextConnections.ContainsKey(threadId))
            {
                DB.ContextConnections.TryAdd(threadId, new List<Connection> { this });
            }
            else
            {
                DB.ContextConnections[threadId].Add(this);
            }

            return this;
        }

        public void Dispose()
        {
            // unregister from context
            var threadId = Thread.CurrentThread.ManagedThreadId;
            if (DB.ContextConnections.ContainsKey(threadId))
            {
                DB.ContextConnections[threadId].Remove(this);
            }

            RawConnection?.Close();

            Log.Debug($"Disconnect from \"{Name}\"");
        }

        #region low level operation

        

        



        internal int DoExecute(string statement, object param, int? timeout = null)
        {
            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // execute
            var affected = RawConnection.Execute(statement, param, Transaction, timeout ?? DB.Config.DefaultTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {affected} recored(s) affected");

            return affected;
        }

        internal IDataReader DoExecuteReader(string statement, object param, int? timeout = null)
        {
            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // execute reader
            var reader = RawConnection.ExecuteReader(statement, param, Transaction, timeout ?? DB.Config.DefaultTimeout, CommandType.Text);

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {reader.RecordsAffected} recored(s) affected");

            return reader;
        }

        #endregion

        #region low level operation async

        

        internal async Task<int> DoExecuteAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // execute
            var affected = await RawConnection.ExecuteAsync(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {affected} recored(s) affected");

            return affected;
        }

        internal async Task<IDataReader> DoExecuteReaderAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            // start stopwatch and log statement
            var stopwatch = Stopwatch.StartNew();
            Log.Debug($"Statement: {statement}");

            // ensure connection
            if (RawConnection.State == ConnectionState.Closed)
            {
                // try open again
                RawConnection.Open();
            }

            // execute reader
            var reader = await RawConnection.ExecuteReaderAsync(new CommandDefinition(statement, param, Transaction, timeout ?? DB.Config.DefaultTimeout, CommandType.Text, CommandFlags.Buffered, cancellationToken));

            // stop stopwatch and log result
            stopwatch.Stop();
            Log.Debug($"Query executed in: {stopwatch.ElapsedMilliseconds}ms, {reader.RecordsAffected} recored(s) affected");

            return reader;
        }

        #endregion
    }
}