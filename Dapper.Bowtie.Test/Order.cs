namespace Dapper.Bowtie.Test
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Completed,
        Cancelled
    }

    public class Order : Entity
    {
        [Column]
        public long UserId { get; set; }

        [Column]
        public decimal totalAmount { get; set; }

        [Column]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}
