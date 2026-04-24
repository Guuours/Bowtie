using System.Linq.Expressions;

namespace Bowtie.Test
{
    public class Tests
    {
        private Connection db;

        [SetUp]
        public void Setup()
        {
            db = DB.Connect();
        }

        public void Where<T1, T2>(Expression<Func<T1, T2, bool>> exp)
        {
            Console.WriteLine(exp.Parameters[0].Name);
            Console.WriteLine(exp.Parameters[1].Name);
        }

        [Test]
        public void Test()
        {
            // simple query, return same type with From call
            var users = DB
              .From<User>()
              .Where(u => u.Age > 18)
              .OrderBy(u => u.Id)
              .Select();

            // mapping query, return different type with From call
            var users1 = DB.Query<UserDTO>()
              .From<User>()
              .Where<User>(u => u.Age > 18)
              .OrderBy<User>(u => u.Age)
              .Select();

            // mapping query with lambda selector, return different type with From call
            var users2 = DB.Query<UserDTO>()
              .From<User>()
              .Where<User>(u => u.Age > 18)
              .OrderBy<User>(u => u.Id)
              .Select<User>(u => new UserDTO());

            // mapping query with multiple tables, return different type with From call
            var users3 = DB.Query<ItemSold>()
              .From<User>()
              .Join<Order, User>((o, u) => u.Id == o.UserId && o.Status == OrderStatus.Paid)
              .LeftJoin<Order, OrderItem>((o, oi) => o.Id == oi.OrderId)
              .Where<Order, User>((o, u) => u.Age > 10 || o.Id > 0)
              .OrderBy<Order>(o => o.Id)
              .OrderByDescending<User>(u => u.Id)
              .Select<Order, OrderItem, User>((o, oi, u) => GetSummary(oi, o, u));

            

            var i = 1;
        }

        public class UserDTO
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class ItemSold
        {
            public long OrderId { get; set; }

            public OrderStatus Status { get; set; }

            public decimal SubTotal { get; set; }

            public string ItemName { get; set; }

            public int Quantity { get; set; }

            public string SoldTo { get; set; }
        }

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

        [TearDown]
        public void TearDown()
        {
            db.Dispose();
        }
    }
}
