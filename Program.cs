namespace MyShop
{
    using Microsoft.EntityFrameworkCore;
    using MyShop.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        public static void Main()
        {
            using (var db = new ShopDbContext())
            {
                //we use method wich to clean and delete; this method will receive the DB
                PrepareDatabase(db);
                SaveSalesmen(db);

                SaveItems(db);
                //2nd part from the task
                ProcessCommands(db);
                //3th part from task
                //PrintSalesmenWithCustomerCount(db);
                //PrintCustomerWithOrdersAndReviewsCount(db);
                //PrintCustomerWithOrdersAndReviews(db);
                //PrintCustomerData(db);
                PrintOrdersWithMoreThanOneItem(db);

            }
        }

        private static void PrepareDatabase(ShopDbContext db)
        {
            //we must to test on clear each time
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        private static void SaveSalesmen(ShopDbContext db)
        {
            var salesmen = Console.ReadLine().Split(';');

            foreach (var salesman in salesmen)
            {
                db.Add(new Salesman { Name = salesman });
            }

            db.SaveChanges();
        }

        private static void ProcessCommands(ShopDbContext db)
        {
            while (true)
            {
                var line = Console.ReadLine();

                if (line == "END")
                {
                    break;
                }

                //and then separated parts Split by '-'

                var parts = line.Split('-');
                //1st part is command, and 2nd ate arguments
                var command = parts[0];
                var arguments = parts[1];

                switch (command)
                {
                    case "register":
                        RegisterCustomer(db, arguments);
                        break;
                    case "order":
                        SaveOrder(db, arguments);
                        break;
                    case "review":
                        SaveReview(db, arguments);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void RegisterCustomer(ShopDbContext db, string arguments)
        {
            var parts = arguments.Split(';');
            var customerName = parts[0];    //1st part
            var salesmanId = int.Parse(parts[1]);          //2nd part

            //we have to add this customer in db
            db.Add(new Customer
            {
                Name = customerName,
                SalesmanId = salesmanId
            });

            db.SaveChanges();
        }

        private static void PrintSalesmenWithCustomerCount(ShopDbContext db)
        {
            // we have to take every salesmen and we can print all names such as counts
            // this Request is not optimising
            //var salesmenData = db.Salesmen.Include(s => s.Customers).ToList();

            var salesmenData = db
                .Salesmen
                .Select(s => new
                {
                    s.Name,
                    Customers = s.Customers.Count
                })
                .OrderByDescending(s => s.Customers)
                .ThenBy(s => s.Name)
                .ToList();

            foreach (var salesman in salesmenData)
            {
                Console.WriteLine($"{salesman.Name} - {salesman.Customers} customers");
            }
        }

        private static void SaveOrder(ShopDbContext db, string arguments)
        {
            var parts = arguments.Split(';');

            var customerId = int.Parse(parts[0]);

            var order = new Order { CustomerId = customerId };

            //var itemIds = new HashSet<int>();

            for (int i = 1; i < parts.Length; i++)
            {
                var itemId = int.Parse(parts[i]);

                order.Items.Add(new ItemOrder
                {
                    ItemId = itemId,
                });
                //itemIds.Add(itemId);
            }

            db.Add(order);

            db.SaveChanges();
        }

        private static void SaveReview(ShopDbContext db, string arguments)
        {
            var parts = arguments.Split(';');

            var customerId = int.Parse(parts[0]);
            //var customerId = int.Parse(arguments);
            var itemId = int.Parse(parts[1]);

            db.Add(new Review
            {
                CustomerId = customerId,
                ItemId = itemId
            });

            db.SaveChanges();
        }

        private static void PrintCustomerWithOrdersAndReviewsCount(ShopDbContext db)
        {
            var CustomersData = db
                .Customers
                .Select(c => new
                {
                    c.Name,
                    Orders = c.Orders.Count,
                    Reviews = c.Reviews.Count
                })
                .OrderByDescending(c => c.Orders)
                .ThenByDescending(c => c.Reviews)
                .ToList();

            foreach (var customer in CustomersData)
            {
                Console.WriteLine(customer.Name);
                Console.WriteLine($"Orders: {customer.Orders}");
                Console.WriteLine($"Reviews: {customer.Reviews}");
            }
        }

        private static void SaveItems(ShopDbContext db)
        {
            while (true)
            {
                var line = Console.ReadLine();

                if (line == "END")
                {
                    break;
                }

                var parts = line.Split(';');
                var itemName = parts[0];
                var itemPrice = decimal.Parse(parts[1]);

                db.Add(new Item
                {
                    Name = itemName,
                    Price = itemPrice
                });
            }
            db.SaveChanges();
        }

        private static void PrintCustomerWithOrdersAndReviews(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            var customerData = db
                .Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    Orders = c.Orders.Select(o => new
                    {
                        o.Id,
                        Items = o.Items.Count
                    })
                    .OrderBy(o => o.Id),
                    Reviews = c.Reviews.Count
                })
                .FirstOrDefault();

            foreach (var order in customerData.Orders)
            {
                Console.WriteLine($"order {order.Id}: {order.Items} items");
            }

            Console.WriteLine($"reviews: {customerData.Reviews}");
        }

        private static void PrintCustomerData(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());
            var customerData = db
                .Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    c.Name,
                    Orders = c.Orders.Count,
                    Reviews = c.Reviews.Count,
                    Salesman = c.Salesman.Name
                })
                .FirstOrDefault();

            Console.WriteLine($"Customer: {customerData.Name}");
            Console.WriteLine($"Orders count: {customerData.Orders}");
            Console.WriteLine($"Reviews count: {customerData.Reviews}");
            Console.WriteLine($"Salesman: {customerData.Salesman}");
        }

        private static void PrintOrdersWithMoreThanOneItem(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            var orders = db
                .Orders
                .Where(o => o.CustomerId == customerId)
                .Where(o =>o.Items.Count > 1)
                .Count();

            Console.WriteLine($"Orders: {orders}");
        }
    }
}
