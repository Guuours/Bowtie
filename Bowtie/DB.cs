using Microsoft.Data.SqlClient;
using MySqlConnector;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Threading;

namespace Bowtie
{
    public partial class DB
    {
        #region config

        private static Config config = null;

        internal static Config Config
        {
            get
            {
                if (config == null)
                {
                    try
                    {
                        config = new Config();
                        var bowtieJsonInfo = new FileInfo(Path.Combine(AppContext.BaseDirectory, "bowtie.json"));
                        var appSettingsJsonInfo = new FileInfo(Path.Combine(AppContext.BaseDirectory, "appsettings.json"));

                        // use bowtie.json
                        if (bowtieJsonInfo.Exists)
                        {
                            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(bowtieJsonInfo.FullName));
                        }
                        // use appsettings.json
                        else if (appSettingsJsonInfo.Exists)
                        {
                            var wrapper = JsonConvert.DeserializeObject<AppSettingsConfigWrapper>(File.ReadAllText(appSettingsJsonInfo.FullName));
                            config = wrapper.Bowtie;
                        }
                        else
                        {
                            throw new Exception("Bowtie config file not found");
                        }

                        if (config == null)
                        {
                            throw new Exception("Bowtie is not properly configured");
                        }
                        else
                        {
                            // set default conn
                            DefaultConnectionName = config.Connections.Find(conn => conn.Default)?.Name;
                            if (DefaultConnectionName == null && config.Connections.Count > 0)
                            {
                                DefaultConnectionName = config.Connections[0].Name;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, ex);
                    }
                }

                return config;
            }
        }

        #endregion

        #region connections

        public static List<NamedConnection> NamedConnections { get; set; } = Config.Connections;

        public static string DefaultConnectionName { get; set; }

        public static Connection Default
        {
            get
            {
                // check if there is context conn available
                var threadId = Thread.CurrentThread.ManagedThreadId;
                if (ContextConnections.ContainsKey(threadId))
                {
                    // using context connection
                    var connectionList = ContextConnections[threadId];
                    if (connectionList.Count > 0)
                    {
                        return connectionList[connectionList.Count - 1];
                    }
                }

                // using default connection
                return Connect();
            }
        }

        public static Connection Connect(string connName = null)
        {
            // check connection count
            if (Config.Connections.Count == 0)
            {
                throw new Exception("No connection is configured");
            }

            // no conn name is given
            if (connName == null)
            {
                connName = DefaultConnectionName;
            }

            // get conn by name
            var namedConn = Config.Connections.Find(cfg => cfg.Name == connName);
            if (namedConn == null)
            {
                throw new Exception($"Connection with name \"{connName}\" does not exist");
            }

            // create conn
            Log.Debug($"Connecting to \"{connName}\"");
            return new Connection
            {
                Name = connName,
                DatabaseType = namedConn.DatabaseType,
                RawConnection = EstablishConnection(namedConn.ConnectionString, namedConn.DatabaseType)
            };
        }

        public static Connection Connect(string connStr, DatabaseType type)
        {
            Log.Debug($"Connecting to \"{connStr}\"");
            // create conn
            return new Connection
            {
                Name = connStr,
                DatabaseType = type,
                RawConnection = EstablishConnection(connStr, type)
            };
        }

        private static DbConnection EstablishConnection(string connStr, DatabaseType type)
        {
            DbConnection conn = null;

            switch (type)
            {
                case DatabaseType.MSSQL:
                case DatabaseType.MSSQL_LEGACY:
                    {
                        conn = new SqlConnection(connStr);
                        break;
                    }
                case DatabaseType.MYSQL:
                    {
                        conn = new MySqlConnection(connStr);
                        break;
                    }
                //case DatabaseType.Oracle:
                //    {
                //        conn = new OracleConnection(connStr);
                //        break;
                //    }
                default:
                    return null;
            }

            conn?.Open();
            return conn;
        }

        #endregion

        internal static ConcurrentDictionary<int, List<Connection>> ContextConnections { get; set; } = new ConcurrentDictionary<int, List<Connection>>();
    }

    public class DZ : DB { }
}