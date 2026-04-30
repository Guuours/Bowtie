```markdown
# Bowtie ORM

> **Note**: This is a very early version of Bowtie ORM. For preview purposes only, use it at your own risk.

Bowtie is a lightweight, fluent Object-Relational Mapper (ORM) for .NET. It makes interacting with your database straightforward by combining basic Active Record patterns with type-safe Lambda Expressions. It targets `.NET Standard 2.0` and `.NET 8`, and supports both MS SQL and MySQL database dialects.

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

You can initialize a connection instance anywhere in your code like this:
```csharp
var db = DB.Connect();  // Uses the first connection from the config by default
var db = DB.Connect("myconn");  // Connects using the named connection from the config
```

## Defining Entities

Map your domain models to database tables by inheriting from `BaseEntity`. Use `[Table]` to specify table names, and `[Column]` to map properties to database columns.

```csharp
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

### Conditional Deletions

Delete records using conditions via the lambda builder:

```csharp
int rowsDeleted = DB.Query<Order>()
  .Where(o => o.Status == OrderStatus.Canceled)
  .Delete();
```
```