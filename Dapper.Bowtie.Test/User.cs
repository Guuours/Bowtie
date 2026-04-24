namespace Dapper.Bowtie.Test
{
    public enum Status
    {
        Approved = 1,
        Rejected = 2,
        Cancelled = 3
    }

    [Table(Alias = "u")]
    public class User : Entity
    {
        [Column]
        public string Name { get; set; }

        [Column]
        public int Age { get; set; }

        [Column]
        public DateTime Birthday { get; set; }

        [Column]
        public decimal? Height { get; set; }

        [Column("Married")]
        public bool IsMarried { get; set; }

        [Column]
        public int? Balance { get; set; }

        [Column]
        public int Friend { get; set; }

        [Column]
        public Status Status { get; set; }

        [Column]
        public Status IntStatus { get; set; }
    }
}
