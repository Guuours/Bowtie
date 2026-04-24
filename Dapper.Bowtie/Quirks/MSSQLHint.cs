namespace Dapper.Bowtie
{
    // mssql with hint
    public enum With
    {
        NOLOCK,
        ROWLOCK,
        UPDLOCK,
        HOLDLOCK,
        FORCESEEK,
        FORCESCAN,
        NOEXPAND
    }
}