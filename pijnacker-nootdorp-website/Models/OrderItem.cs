public class OrderItem
{
    public int OrderItemId { get; set; }

    public int HouseId { get; set; }
    public int OrderId { get; set; }

    public virtual House House { get; set; }
    public virtual Order Order { get; set; }
}