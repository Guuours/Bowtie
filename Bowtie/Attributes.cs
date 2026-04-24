using System;

namespace Bowtie
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string Name { get; set; }

        public TableAttribute() { }

        public TableAttribute(string name)
        {
            Name = name;
        }
    }

    [Flags]
    public enum When
    {
        Select = 1,
        Insert = 2,
        Update = 4
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public When Ignore { get; set; }

        public bool PK { get; set; } = false;

        public ColumnAttribute() { }

        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }
}