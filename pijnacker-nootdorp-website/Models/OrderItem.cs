public class OrderItem
{
    public int Id { get; set; }

    public int House_Id { get; set; }
    public int Order_Id { get; set; }

    public House House { get; set; }
    public Order Order { get; set; }
}