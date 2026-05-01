using Serilog;
using System;
using System.Collections.Generic;

namespace Bowtie
{
    public class Factory : IDisposable
    {
        private List<Connection> Connections { get; set; } = new List<Connection>();

        public Connection Connect(string connName)
        {
            var conn = DB.Connect(connName).KeepAlive();
            Connections.Add(conn);
            return conn;
        }

        public void Dispose()
        {
            foreach (var connection in Connections)
            {
                connection.Dispose();
            }

            Log.Debug($"Bowtie factory disposed, {Connections.Count} connection(s) destroyed");
        }
    }
}