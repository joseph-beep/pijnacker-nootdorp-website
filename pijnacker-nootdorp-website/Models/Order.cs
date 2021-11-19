using System.Collections.Generic;

public class Order
{
    public int Id { get; set; }

    public int User_Id { get; set; }

    public User User { get; set; }

    public ICollection<OrderItem> Items { get; set; }
}
