using System.Collections.Generic;

public class HomeModel
{
    public User User { get; set; }
    public List<House> Houses { get; set; }

    public int MinimumPrice { get; set; }
    public int MaximumPrice { get; set; }
}