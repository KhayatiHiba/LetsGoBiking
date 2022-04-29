using System;
using System.ServiceModel;

namespace RoutingService;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Starting of the routing service !");
        var service = new ServiceHost(typeof(Service1));
        service.Open();
        Console.ReadLine();
    }
}