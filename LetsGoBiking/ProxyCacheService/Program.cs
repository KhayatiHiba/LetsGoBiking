using System;
using System.ServiceModel;

namespace ProxyCacheService
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("Starting of the proxy service !");
            var service = new ServiceHost(typeof(Service1));
            service.Open();
            Console.ReadLine();
        }
    }
}