```markdown
# Bowtie ORM

**Note**: This is a very early version of Bowtie ORM. For preview purposes only
Some of the APIs are still in flux and may change without warning.
And some of the features are not fully implemented or fully tested yet.
Please use it at your own risk.

Bowtie is a lightweight, fluent Object-Relational Mapper (ORM) for .NET.
It makes interacting with your database straightforward by combining basic Active Record patterns with type-safe Lambda Expressions.
It targets `.NET Standard 2.0` and aiming to provide max compatibility, and supports both SQLServer and MySQL database dialects.

## Configuration

Bowtie automatically reads database connection settings from either `bowtie.json` or `appsettings.json` located in the application's root directory:

```json
{
  "connections": [
    {
      "name": "myconn",
      "connectionString": "Server=localhost;Database=mydb;Uid=sa;Pwd=mypassword;",
      "databaseType": "MYSQL" // Supported values: MYSQL, MSSQL
    }
  ]
} 
```

## Connection Management

You can initialize a connection instance anywhere in your code like this:
```csharp
var db = DB.Connect();  // Uses the first connection from the config by default
var db = DB.Connect("myconn");  // Connects using the named connection from the config
```

And the connection would be released after the first query is executed.
To keep the connection alive for multiple operations, you can call `db.KeepAlive()` to prevent auto-disposal after the first query, and then call `db.Dispose()` manually when you're done with all your database operations.
```csharp
var db = DB.Connect().KeepAlive();
// your database operations here
db.Dispose();
```

or you can use `using` statement to automatically dispose the connection when it's out of scope:
```csharp
using (var db = DB.Connect())
{
    // your database operations here
}
```

### Connection Auto-Disposal

When using functions directly from DB class, the database connection would be automatically created and disposed for you. For example:

```csharp
var users = DB.From<User>()
  .Where(u => u.Age > 18)
  .Select();
```

## Defining Entities

Map your domain models to database tables by inheriting from `BaseEntity`. Use `[Table]` to specify table names, and `[Column]` to map properties to database columns.

```csharp
// Maps this class to the "orders" table in the database
// If the table name is the same as the class name, you can omit the attribute
[Table("orders")]
public class Order : BaseEntity
{
    [Column(PK = true, Ignore = When.Insert | When.Update)]
    public int Id { get; set; }

    [Column("user_id")]
    public long UserId { get; set; }

    [Column("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column("status")]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    // use Ignore attribute to exclude this property from being included in INSERT or UPDATE statements
    [Column("created_at", Ignore = When.Update)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // IsNew determines whether calling Save() generates an INSERT or UPDATE query
    public override bool IsNew(Connection conn = null)
    {
        return Id == 0;
    }
}
```

## Creating, Updating, and Deleting (Active Record)

Using the `BaseEntity` allows you to call `.Save()` directly on your objects. Bowtie executes either an `INSERT` or `UPDATE` depending on your `IsNew()` logic.

```csharp
var myOrder = new Order { UserId = 1, TotalAmount = 99.99m };
myOrder.Save(); // Evaluates IsNew() which returns true (Id == 0), executing INSERT

myOrder.TotalAmount = 89.99m;
myOrder.Save(); // Id != 0, executing UPDATE
```

## Querying Data (Raw SQL)

If you prefer writing raw SQL, Bowtie provides a simple API for that as well. You can execute any SQL query and map the results back to your entities or DTOs.

```csharp
var users = DB.Query<User>("SELECT * FROM users WHERE age > @age", new { age = 18 });
```

And for count and pagination, you can just call the corresponding methods with normal select queries
without worrying about the underlying SQL generation for count and pagination, Bowtie will handle that for you.

```csharp
int totalUsers = DB.Count("SELECT * FROM users WHERE age > @age", new { age = 18 });
var pagedUsers = DB.SelectPage<User>(2, 10, "SELECT * FROM users WHERE age > @age", new { age = 18 });
```

## Querying Data (Lambda Expressions)

Bowtie's lambda query API allows you to safely construct complex SQL queries entirely in C# without string manipulation. Data can be seamlessly mapped back into DTOs or original Models.

### Simple Queries

If you only need to return the entity type being queried, you can omit the generic parameter from the initial DB call.
```csharp
var users = DB
  .From<User>()
  .Where(u => u.Age > 18)
  .OrderBy(u => u.Id)
  .Select();
```

### Mapping Queries (DTOs)

If you are querying a table but wish to map the results into a different type (like a DTO), specify the output type in `DB.Query<T>()`.
```csharp
var users1 = DB.Query<UserDTO>()
  .From<User>()
  .Where<User>(u => u.Age > 18)
  .OrderBy<User>(u => u.Age)
  .Select();
```

### Advanced Joins & Selectors

You can perform robust inner or left joins across multiple tables smoothly. You can define a selector function to tell Bowtie exactly how to hydrate your output objects.

```csharp
var summaries = DB.Query<ItemSold>()
  .From<User>()
  .Join<Order, User>((o, u) => u.Id == o.UserId && o.Status == OrderStatus.Paid)
  .LeftJoin<Order, OrderItem>((o, oi) => o.Id == oi.OrderId)
  .Where<Order, User>((o, u) => u.Age > 10 || o.Id > 0)
  .OrderBy<Order>(o => o.Id)
  .OrderByDescending<User>(u => u.Id)
  .Select<Order, OrderItem, User>((o, oi, u) => GetSummary(oi, o, u));

// Mapper example for the query above
public ItemSold GetSummary(OrderItem oi, Order o, User u)
{
    return new ItemSold
    {
        OrderId = oi.OrderId,
        Status = o.Status,
        SubTotal = oi.Quantity * oi.Price,
        ItemName = oi.Goods,
        Quantity = oi.Quantity,
        SoldTo = u.Name
    };
}
```

### Updating Records

Update records using conditions via the lambda builder:

```csharp
db.From<User>()
    .Join<Order, User>((o, u) => o.UserId == u.Id && o.Status == OrderStatus.Paid)
    .Set(u => u.Name == "test")
    .Where(u => u.Age > 18)
    .Update();
```

### Deleting Records

Delete records using conditions via the lambda builder:

```csharp
db.From<User>()
    .Join<User, Order>((u, o)=>u.Id == o.UserId)
    .Where<Order, User>((o, u) => u.Age > 60 && o.Status == OrderStatus.Paid)
    .Delete();
```