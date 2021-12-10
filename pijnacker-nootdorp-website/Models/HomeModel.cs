using System.Collections.Generic;

public class HomeModel
{
    public User User { get; set; }
    public List<House> Houses { get; set; }

    public bool IsInitialized { get; set; }
    public int Price_Minimum { get; set; }
    public int Price_Maximum { get; set; }
    public int OutdoorArea_Minimum { get; set; }
    public int OutdoorArea_Maximum { get; set; }
}