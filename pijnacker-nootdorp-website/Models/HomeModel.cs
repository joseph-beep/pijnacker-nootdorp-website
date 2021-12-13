using System.Collections.Generic;

public class HomeModel
{
    public User User { get; set; }
    public List<House> Houses { get; set; }

    public bool IsInitialized { get; set; }
    public string SearchQuery { get; set; }
    public string Price_Minimum { get; set; }
    public string Price_Maximum { get; set; }
    public string OutdoorArea_Minimum { get; set; }
    public string OutdoorArea_Maximum { get; set; }
    public string IndoorArea_Minimum { get; set; }
    public string IndoorArea_Maximum { get; set; }

    public string Wheelchair { get; set; }
    public string Car { get; set; }
    public string PublicTransport { get; set; }
}