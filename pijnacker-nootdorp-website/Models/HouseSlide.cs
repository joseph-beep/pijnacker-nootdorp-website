public class HouseSlide
{
    public int Id { get; set; }
    public int HouseId { get; set; }
    public string Subtitle { get; set; }
    public string Picture { get; set; }

    public virtual House House { get; set; }
}
