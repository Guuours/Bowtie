# Note: This is a very early version of Bowtie ORM
For preview purpose only, use it at your own risk

# Config bowtie.json
```
{
	"connections": [
		{
			"name": "myconn",
			"connectionString": {your conn str},
			"databaseType": "MYSQL"
		}
	]
} 
```

# Entity Model
```
public class Order : Entity
{
    [Column]
    public long UserId { get; set; }

    [Column]
    public decimal totalAmount { get; set; }

    [Column]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}
```

# Select with Lambda
```
var summaries = DB.Query<ItemSold>()
  .From<User>()
  .Join<Order, User>((o, u) => u.Id == o.UserId && o.Status == OrderStatus.Paid)
  .LeftJoin<Order, OrderItem>((o, oi) => o.Id == oi.OrderId)
  .Where<Order, User>((o, u) => u.Age > 10 || o.Id > 0)
  .OrderBy<Order>(o => o.Id)
  .OrderByDescending<User>(u => u.Id)
  .Select<Order, OrderItem, User>((o, oi, u) => GetSummary(oi, o, u));
```