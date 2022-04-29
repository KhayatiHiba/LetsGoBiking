using System.Collections.Generic;
using System.Device.Location;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RoutingService;

public class CustomGeoCoordinate : GeoCoordinate
{
    public CustomGeoCoordinate(double latitude, double longitude) : base(latitude, longitude)
    {
    }

    public override string ToString()
    {
        return base.Longitude.ToString().Replace(',', '.') + "," + base.Latitude.ToString().Replace(',', '.');
    }
}

public class Coordinate
{
    public double[] coordinates { get; }

    private CustomGeoCoordinate _geoCoordinate;

    public Coordinate(double[] values)
    {
        coordinates = values;
    }

    public CustomGeoCoordinate ToGeoCoordinate()
    {
        return _geoCoordinate ??= new CustomGeoCoordinate(coordinates[1], coordinates[0]);
    }
}

public class Result
{
    public List<Path> routes { get; set; }
    public bool hasError { get; set; }
    public string message { get; set; }
    
    public Result()
    {
        routes = new List<Path>();
        this.hasError = false;
        this.message = "";
    }

    public Result(bool hasError, string message)
    {
        this.hasError = hasError;
        this.message = message;
    }

    public void AddRoute(Path route)
    {
        routes.Add(route);
    }
}
public class Path
{
    public Properties properties { get; set; }
    public Geometry geometry { get; set; }
   
    public Path()
    {
        this.properties = null;
        this.geometry = null;

    }
    
    public Path(Properties properties, Geometry geometry)
    {
        this.properties = properties;
        this.geometry = geometry;
    }
}

public class Properties
{
    public Summary summary { get; set; }

    public Segment[] segments { get; set; }
}

public class Summary
{
    public double distance { get; set; }
    public double duration { get; set; }
}

public class Segment
{
    public Step[] steps { get; set; }
}

public class Step
{
    public double distance { get; set; }
    public double duration { get; set; }
    public string instruction { get; set; }

}

public class Geometry
{
    public double[][] coordinates { get; set; }
}

public class Response
{
    public Feature[] features { get; set; }
}

public class Feature
{
    public Geometry geometry { get; set; }
    public Properties properties { set; get; }
}