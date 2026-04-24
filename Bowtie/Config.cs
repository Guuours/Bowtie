using System.Collections.Generic;

namespace Bowtie
{
    public enum ExceptionLevel
    {
        None,
        WarningOnly,
        ErrorOnly,
        All
    }

    public class Config
    {
        public ExceptionLevel ExceptionLevel = ExceptionLevel.ErrorOnly;

        public List<NamedConnection> Connections { get; set; } = new List<NamedConnection>();

        public int DefaultTimeout { get; set; } = 30;
    }

    public class NamedConnection
    {
        public string Name { get; set; }

        public string ConnectionString { get; set; }

        public DatabaseType DatabaseType { get; set; }

        public bool Default { get; set; }
    }

    internal class AppSettingsConfigWrapper
    {
        public Config Accessor { get; set; }
    }
}