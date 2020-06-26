namespace MyShop.Data
{
    using System.Collections.Generic;

    public class Order
    {
        public int Id { get; set;  }

        public int CustomerId { get; set;  }

        public Customer Customer { get; set;  }
        //now, we have to add many count of orders in Customer.class

        //each Order has

        public List<ItemOrder> Items { get; set; } = new List<ItemOrder>();
        //now to create Many to Many relation we need on new intermediate table ItemOrder :)
    }
}
