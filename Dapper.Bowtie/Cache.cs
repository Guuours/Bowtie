using System;
using System.Collections.Concurrent;

namespace Dapper.Bowtie
{
    internal static class Cache
    {
        internal static ConcurrentDictionary<Guid, bool> TypeMappings { get; set; } = new ConcurrentDictionary<Guid, bool>();

        internal static ConcurrentDictionary<string, string> Statements { get; set; } = new ConcurrentDictionary<string, string>();
    }
}