namespace MyShop.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ShopDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set;  }

        public DbSet<Salesman> Salesmen { get; set;  }

        public DbSet<Order> Orders { get; set;  }

        public DbSet<Review> Reviews { get; set;  }

        public DbSet<Item> Items { get; set;  }

        //db
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=HOME\\SQLEXPRESS;Database=MyTestShop;Integrated Security=True;");
        }
        
        //relations between tables
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder
                .Entity<Customer>()
                .HasOne(s => s.Salesman)
                .WithMany(s => s.Customers)
                .HasForeignKey(c => c.SalesmanId);

            builder
                .Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId);

            builder
                .Entity<Review>()
                .HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId);
            //now we go in Program.cs to add two more commands :)

            builder
                .Entity<ItemOrder>()
                .HasKey(io => new { io.ItemId, io.OrderId });

            builder
                .Entity<Item>()
                .HasMany(i => i.Orders)
                .WithOne(io => io.Item)
                .HasForeignKey(i => i.ItemId);

            builder
                .Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(io => io.Order)
                .HasForeignKey(io => io.OrderId);

            builder
                .Entity<Item>()
                .HasMany(i => i.Reviews)
                .WithOne(r => r.Item)
                .HasForeignKey(r => r.ItemId);
        }
    }
}
