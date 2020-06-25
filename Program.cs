namespace MyShop
{
    using Microsoft.EntityFrameworkCore;
    using MyShop.Data;
    using System;
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
                //2nd part from the task
                ProcessCommands(db);
                //3th part from task
                PrintSalesmenWithCustomerCount(db);
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
                Console.WriteLine($"{salesman.Name} - {salesman.Customers} customers" );
            }
        }
    }
}
