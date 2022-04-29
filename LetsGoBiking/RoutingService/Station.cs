using Newtonsoft.Json.Linq;

namespace RoutingService;

public class Station
{
    public int number { get; set; }
    public string contractName { get; set; }
    public string name { get; set; }
    public Position position { get; set; }
    public Stands totalStands { get; set; }
    
    public Path route { get; set; }

    public override bool Equals(object obj)
    {
        if (obj is not Station station)
            return false;

        return number == station.number && contractName == station.contractName;
    }}

public class Stands
{
    public Availability availabilities { get; set; }
    public int capacity { get; set; }
}

public class Availability
{
    public int bikes { get; set; }
    public int stands { get; set; }
    public int mechanicalBikes { get; set; }
    public int electricalBikes { get; set; }
}

public class Position
{
    public double latitude { get; set; }
    public double longitude { get; set; }

    private CustomGeoCoordinate _geoCoordinate;

    public CustomGeoCoordinate ToGeoCoordinate()
    {
        return _geoCoordinate ??= new CustomGeoCoordinate(latitude, longitude);
    }

    public override string ToString()
    {
        return (latitude + "").Replace(',', '.') + "," + (longitude + "").Replace(',', '.');
    }
}