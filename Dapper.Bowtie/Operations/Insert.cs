using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public long Insert(BaseEntity entry, int? timeout = null)
        {
            return Insert(entry, null, timeout);
        }

        public long Insert(BaseEntity entry, string tableName, int? timeout = null)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetInsertStatement(entry.GetType(), tableName, DatabaseType);
                // execute
                var identity = 0;
                switch (DatabaseType)
                {
                    case DatabaseType.MSSQL:
                    case DatabaseType.MSSQL_LEGACY:
                    case DatabaseType.MYSQL:
                        {
                            using (var reader = DoExecuteReader(statement, entry, timeout))
                            {
                                var affected = reader.RecordsAffected;
                                if (reader.Read())
                                {
                                    identity = reader.GetInt32(0);
                                }
                            }
                            break;
                        }
                }

                return identity;
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

        public long Insert<T>(object param, int? timeout = null)
        {
            return Insert<T>(param, null, timeout);
        }

        public long Insert<T>(object param, string tableName, int? timeout = null)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetInsertStatement(typeof(T), tableName, DatabaseType);
                // execute
                var identity = 0;
                switch (DatabaseType)
                {
                    case DatabaseType.MSSQL:
                    case DatabaseType.MSSQL_LEGACY:
                    case DatabaseType.MYSQL:
                        {
                            using (var reader = DoExecuteReader(statement, param, timeout))
                            {
                                var affected = reader.RecordsAffected;
                                if (reader.Read())
                                {
                                    identity = reader.GetInt32(0);
                                }
                            }
                            break;
                        }
                }

                return identity;
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

        public long Insert(object param, string tableName, int? timeout = null)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetInsertStatement(param.GetType(), tableName, DatabaseType);
                // execute
                var identity = 0;
                switch (DatabaseType)
                {
                    case DatabaseType.MSSQL:
                    case DatabaseType.MSSQL_LEGACY:
                    case DatabaseType.MYSQL:
                        {
                            using (var reader = DoExecuteReader(statement, param, timeout))
                            {
                                var affected = reader.RecordsAffected;
                                if (reader.Read())
                                {
                                    identity = reader.GetInt32(0);
                                }
                            }
                            break;
                        }
                }

                return identity;
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

        public async Task<long> InsertAsync(BaseEntity entry, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetInsertStatement(entry.GetType(), null, DatabaseType);
                // execute
                long identity = 0;
                switch (DatabaseType)
                {
                    case DatabaseType.MSSQL:
                    case DatabaseType.MSSQL_LEGACY:
                    case DatabaseType.MYSQL:
                        {
                            using (var reader = await DoExecuteReaderAsync(statement, entry, timeout, cancellationToken))
                            {
                                var affected = reader.RecordsAffected;
                                if (reader.Read())
                                {
                                    identity = reader.GetInt64(0);
                                }
                            }
                            break;
                        }
                }

                return identity;
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

        public async Task<long> InsertAsync(BaseEntity entry, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetInsertStatement(entry.GetType(), tableName, DatabaseType);
                // execute
                long identity = 0;
                switch (DatabaseType)
                {
                    case DatabaseType.MSSQL:
                    case DatabaseType.MSSQL_LEGACY:
                    case DatabaseType.MYSQL:
                        {
                            using (var reader = await DoExecuteReaderAsync(statement, entry, timeout, cancellationToken))
                            {
                                var affected = reader.RecordsAffected;
                                if (reader.Read())
                                {
                                    identity = reader.GetInt64(0);
                                }
                            }
                            break;
                        }
                }

                return identity;
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

        public async Task<int> InsertAsync<T>(object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await InsertAsync(param, null, timeout, cancellationToken);
        }

        public async Task<int> InsertAsync<T>(object param, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetInsertStatement(typeof(T), tableName, DatabaseType);
                // execute
                var identity = 0;
                switch (DatabaseType)
                {
                    case DatabaseType.MSSQL:
                    case DatabaseType.MSSQL_LEGACY:
                    case DatabaseType.MYSQL:
                        {
                            using (var reader = await DoExecuteReaderAsync(statement, param, timeout, cancellationToken))
                            {
                                var affected = reader.RecordsAffected;
                                if (reader.Read())
                                {
                                    identity = reader.GetInt32(0);
                                }
                            }
                            break;
                        }
                }

                return identity;
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

        public async Task<int> InsertAsync(object param, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // construct statement
                var statement = SyntaxConstructor.GetInsertStatement(param.GetType(), tableName, DatabaseType);
                // execute
                var identity = 0;
                switch (DatabaseType)
                {
                    case DatabaseType.MSSQL:
                    case DatabaseType.MSSQL_LEGACY:
                    case DatabaseType.MYSQL:
                        {
                            using (var reader = await DoExecuteReaderAsync(statement, param, timeout, cancellationToken))
                            {
                                var affected = reader.RecordsAffected;
                                if (reader.Read())
                                {
                                    identity = reader.GetInt32(0);
                                }
                            }
                            break;
                        }
                }

                return identity;
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

        public static long Insert(BaseEntity entry, int? timeout = null)
        {
            return Default.Insert(entry, timeout);
        }

        public static long Insert(BaseEntity entry, string tableName, int? timeout = null)
        {
            return Default.Insert(entry, tableName, timeout);
        }

        public static long Insert<T>(object param, int? timeout = null)
        {
            return Default.Insert<T>(param, timeout);
        }

        public static long Insert<T>(object param, string tableName, int? timeout = null)
        {
            return Default.Insert<T>(param, tableName, timeout);
        }

        public static long Insert(object param, string tableName, int? timeout = null)
        {
            return Default.Insert(param, tableName, timeout);
        }

        #endregion

        #region async

        public static async Task<long> InsertAsync(BaseEntity entry, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.InsertAsync(entry, timeout, cancellationToken);
        }

        public static async Task<long> InsertAsync(BaseEntity entry, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.InsertAsync(entry, tableName, timeout, cancellationToken);
        }

        public static async Task<long> InsertAsync<T>(object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.InsertAsync<T>(param, timeout, cancellationToken);
        }

        public static async Task<long> InsertAsync<T>(object param, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.InsertAsync<T>(param, tableName, timeout, cancellationToken);
        }

        public static async Task<long> InsertAsync(object param, string tableName, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.InsertAsync(param, tableName, timeout, cancellationToken);
        }

        #endregion
    }
}