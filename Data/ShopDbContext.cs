namespace MyShop.Data
{
    using Microsoft.EntityFrameworkCore;

    public class ShopDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set;  }

        public DbSet<Salesman> Salesmen { get; set;  }

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
        }
    }
}
