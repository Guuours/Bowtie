using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bowtie
{
    public partial class Connection : IDisposable
    {
        #region sync

        public int Count(string statement, int? timeout = null)
        {
            return Count(statement, null, timeout);
        }

        public int Count(string statement, object param, int? timeout = null)
        {
            return FirstOrDefault<int>(SyntaxConstructor.GetCountStatement(statement, DatabaseType), param, timeout);
        }

        #endregion

        #region async

        public async Task<int> CountAsync(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await CountAsync(statement, null, timeout, cancellationToken);
        }

        public async Task<int> CountAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await FirstOrDefaultAsync<int>(SyntaxConstructor.GetCountStatement(statement, DatabaseType), param, timeout, cancellationToken);
        }

        #endregion
    }

    public partial class DB
    {
        #region sync

        public static int Count(string statement, int? timeout = null)
        {
            return Default.Count(statement, timeout);
        }

        public static int Count(string statement, object param, int? timeout = null)
        {
            return Default.Count(statement, param, timeout);
        }

        #endregion

        #region async

        public static async Task<int> CountAsync(string statement, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.CountAsync(statement, timeout, cancellationToken);
        }

        public static async Task<int> CountAsync(string statement, object param, int? timeout = null, CancellationToken cancellationToken = default)
        {
            return await Default.CountAsync(statement, param, timeout, cancellationToken);
        }

        #endregion
    }
}