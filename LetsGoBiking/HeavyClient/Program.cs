using System;
using System.Diagnostics;
using RoutingService;

namespace HeavyClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting of the heavy client !");
            Console.WriteLine("Press any key to start measures :");
            Console.ReadLine();

            Console.WriteLine("\n ------- STARTING TESTS -------");


            Stopwatch stopwatch = new();
            Service1Client client = new Service1Client();

            stopwatch.Start();
            var _ = await client.ComputePathAsync("paris", "nice");
            stopwatch.Stop();

            Console.WriteLine("Elapsed time without proxy is {0} s", (stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.####"));

            for (int i = 0; i < 2; i++)
            {
                stopwatch.Restart();
                _ = await client.ComputePathAsync("paris", "nice");
                stopwatch.Stop();
                Console.WriteLine("Elapsed time with proxy is {0} s", (stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.####"));
            }

            Console.WriteLine("\n ------- TESTS ARE OVER -------");
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}