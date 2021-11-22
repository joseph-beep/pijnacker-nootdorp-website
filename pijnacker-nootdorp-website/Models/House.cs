public class House
{
    public enum Accessibility { Wheelchair, Car, PublicTransport }

    public int Id { get; set; }

    public string Address { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }

    public int Access { get; set; }

    public int BuildYear { get; set; }

    public float OutdoorArea { get; set; }
    public float IndoorArea { get; set; }

    public string Rooms { get; set; }
}