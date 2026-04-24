using Serilog;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Dapper.Bowtie
{
    public partial class Connection : IDisposable
    {
        public void BeginTransaction(Action<Connection> action)
        {
            var oldAlive = AutoRelease;

            try
            {
                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }
                // keep alive during transaction
                AutoRelease = false;
                // begin transaction
                Transaction = RawConnection.BeginTransaction();

                action(this);

                Transaction.Commit();
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                Log.Error(ex, ex.Message);
            }
            finally
            {
                // clear context transaction
                Transaction.Dispose();
                Transaction = null;

                // recover old status
                AutoRelease = oldAlive;

                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        public async Task BeginTransactionAsync(Func<Connection, Task> action)
        {
            var oldAlive = AutoRelease;

            try
            {
                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }
                // keep alive during transaction
                AutoRelease = false;
                // begin transaction
                Transaction = RawConnection.BeginTransaction();

                await action(this);

                Transaction.Commit();
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                Log.Error(ex, ex.Message);
            }
            finally
            {
                // clear context transaction
                Transaction.Dispose();
                Transaction = null;

                // recover old status
                AutoRelease = oldAlive;

                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        public void BeginTransaction(IsolationLevel level, Action<Connection> action)
        {
            var oldStatus = AutoRelease;

            try
            {
                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }
                // keep alive during transaction
                AutoRelease = false;
                // begin transaction
                Transaction = RawConnection.BeginTransaction(level);

                action(this);

                Transaction.Commit();
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                Log.Error(ex, ex.Message);
            }
            finally
            {
                // clear context transaction
                Transaction.Dispose();
                Transaction = null;

                // recover old status
                AutoRelease = oldStatus;

                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }

        public async Task BeginTransactionAsync(IsolationLevel level, Func<Connection, Task> action)
        {
            var oldStatus = AutoRelease;

            try
            {
                // ensure connection
                if (RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    RawConnection.Open();
                }
                // keep alive during transaction
                AutoRelease = false;
                // begin transaction
                Transaction = RawConnection.BeginTransaction(level);

                await action(this);

                Transaction.Commit();
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                Log.Error(ex, ex.Message);
            }
            finally
            {
                // clear context transaction
                Transaction.Dispose();
                Transaction = null;

                // recover old status
                AutoRelease = oldStatus;

                if (AutoRelease)
                {
                    Dispose();
                }
            }
        }
    }

    public partial class DB
    {
        public void BeginTransaction(Action<Connection> action)
        {
            Default.BeginTransaction(action);
        }

        public async Task BeginTransactionAsync(Func<Connection, Task> action)
        {
            await Default.BeginTransactionAsync(action);
        }

        public void BeginTransaction(IsolationLevel level, Action<Connection> action)
        {
            Default.BeginTransaction(level, action);
        }

        public async Task BeginTransactionAsync(IsolationLevel level, Func<Connection, Task> action)
        {
            await Default.BeginTransactionAsync(level, action);
        }

        public static void CrossTransaction(Connection connection1, Connection connection2, Action<Connection, Connection> action)
        {
            try
            {
                if (connection1.RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    connection1.RawConnection.Open();
                }
                connection1.AutoRelease = false;
                connection1.Transaction = connection1.RawConnection.BeginTransaction();

                if (connection2.RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    connection2.RawConnection.Open();
                }
                connection2.AutoRelease = false;
                connection2.Transaction = connection2.RawConnection.BeginTransaction();

                action(connection1, connection2);

                connection1.Transaction.Commit();
                connection2.Transaction.Commit();
            }
            catch (Exception ex)
            {
                connection1.Transaction.Rollback();
                connection2.Transaction.Rollback();

                Log.Error(ex, ex.Message);
            }
            finally
            {
                connection1.Transaction.Dispose();
                connection1.Transaction = null;
                connection2.Transaction.Dispose();
                connection2.Transaction = null;
                connection1.RawConnection?.Close();
                connection2.RawConnection?.Close();
            }
        }

        public static void CrossTransaction(IsolationLevel level, Connection connection1, Connection connection2, Action<Connection, Connection> action)
        {
            try
            {
                if (connection1.RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    connection1.RawConnection.Open();
                }
                connection1.AutoRelease = false;
                connection1.Transaction = connection1.RawConnection.BeginTransaction(level);

                if (connection2.RawConnection.State == ConnectionState.Closed)
                {
                    // try open again
                    connection2.RawConnection.Open();
                }
                connection2.AutoRelease = false;
                connection2.Transaction = connection2.RawConnection.BeginTransaction(level);

                action(connection1, connection2);

                connection1.Transaction.Commit();
                connection2.Transaction.Commit();
            }
            catch (Exception ex)
            {
                connection1.Transaction.Rollback();
                connection2.Transaction.Rollback();

                Log.Error(ex, ex.Message);
            }
            finally
            {
                connection1.Transaction.Dispose();
                connection1.Transaction = null;
                connection2.Transaction.Dispose();
                connection2.Transaction = null;
                connection1.RawConnection?.Close();
                connection2.RawConnection?.Close();
            }
        }
    }
}