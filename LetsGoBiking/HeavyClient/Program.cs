using System;
using System.Diagnostics;
using ServiceReference1;

namespace HeavyClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Stopwatch stopwatch = new();
            Service1Client client = new Service1Client();


            Console.WriteLine("Starting of the heavy client !");
            while (true)
            {
                Console.WriteLine("Press enter key to start. Tap exit to quit");
                var value = Console.ReadLine();
                if (value == "exit")
                {
                    break;
                }
                Console.WriteLine("Where are you located ?");

                var location = Console.ReadLine();

                Console.WriteLine("Where are you headed ?");

                var destination = Console.ReadLine();

                stopwatch.Restart();
                var _ = await client.ComputePathAsync(location, destination);
                stopwatch.Stop();

                Console.WriteLine("Elapsed time is {0} s", (stopwatch.ElapsedMilliseconds / 1000.0).ToString("0.####"));
            }
        }
    }
}