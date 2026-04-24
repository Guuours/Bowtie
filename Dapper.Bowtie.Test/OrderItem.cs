namespace Dapper.Bowtie.Test
{
    public class OrderItem : Entity
    {
        [Column]
        public long OrderId { get; set; }

        [Column]
        public string Goods { get; set; }

        [Column]
        public decimal Price { get; set; }

        [Column]
        public string Currency { get; set; }

        [Column]
        public int Quantity { get; set; }
    }
}
