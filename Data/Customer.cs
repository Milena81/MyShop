namespace MyShop.Data
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Customer 
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]

        public string Name { get; set; }

        public int SalesmanId { get; set; }

        public Salesman Salesman { get; set;  }

        public List<Order> Orders { get; set; } = new List<Order>();

        //to work this connesction, now we have to add builder in ShopDbContext.cs in protect method OnModelCreating, such as add DbSet...

        //add one more collection for Review

        public List<Review> Reviews { get; set; } = new List<Review>(); // next builder...
    }
}
