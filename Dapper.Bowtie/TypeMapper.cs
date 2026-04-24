using Dapper;
using System;
using System.Linq;
using System.Reflection;

namespace Dapper.Bowtie
{
    internal static class TypeMapper
    {
        internal static void Map(Type type)
        {
            // skip if already mapped
            if (Cache.TypeMappings.ContainsKey(type.GUID)) { return; }

            SqlMapper.SetTypeMap(type, new CustomPropertyTypeMap(type,
                (innerType, columnName) =>
                {
                    var props = innerType.GetProperties();

                    // exactly match with name in col attr
                    var propInfo = props.FirstOrDefault(prop =>
                    {
                        var colAttr = prop.GetCustomAttribute<ColumnAttribute>(false);
                        var nameInAttr = colAttr?.Name ?? colAttr?.Name;

                        return columnName.ToLower() == nameInAttr?.ToLower();
                    });
                    if (propInfo != null) return propInfo;

                    // exactly match with prop name
                    propInfo = props.FirstOrDefault(prop =>
                    {
                        return columnName == prop.Name;
                    });
                    if (propInfo != null) return propInfo;

                    // match by sequence
                    propInfo = props.FirstOrDefault(prop =>
                    {
                        return columnName.ToLower() == prop.Name.ToLower();
                    });

                    return propInfo;
                })
            );

            // add mapped flag in cache
            Cache.TypeMappings.TryAdd(type.GUID, true);
        }
    }
}