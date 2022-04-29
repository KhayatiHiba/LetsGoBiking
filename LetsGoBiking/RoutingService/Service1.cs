using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static RoutingService.RequestHelper;

namespace RoutingService
{
    public class Service1 : IService1
    {
        public async Task<Result> ComputePath(string location, string destination)
        {
            Console.WriteLine("Finding path between {0} and {1}...", location, destination);
            Result result = new Result();

            try
            {
                Console.WriteLine("Retrieving coordinates...");
                Coordinate sCoord = await GetCoordinate(location);
                Coordinate eCoord = await GetCoordinate(destination);
                if (sCoord == null || eCoord == null)
                {
                    result.hasError = true;
                    result.message = "Could not find coordinates for start and/or end. Please check your input.";
                    return result;
                }

                CustomGeoCoordinate startCoordinate = sCoord.ToGeoCoordinate();
                CustomGeoCoordinate endCoordinate = eCoord.ToGeoCoordinate();
                Console.WriteLine("Coordination retreived : ");
                Console.WriteLine("Start : {0}", startCoordinate);
                Console.WriteLine("End : {0}", endCoordinate);
                Console.WriteLine("Looking for bike stations...");

                Station startStation = await GetClosestStation(startCoordinate);
                if (startStation is null)
                {
                    Console.WriteLine("No stations found, returning walking itinerary.");
                    result.AddRoute(await GetPath(startCoordinate, endCoordinate, "foot-walking"));
                    return result;
                }

                Station endStation = await GetClosestStation(endCoordinate, false);
                if (endStation is null || startStation.Equals(endStation))
                {
                    Console.WriteLine("No stations found, returning walking itinerary.");
                    result.AddRoute(await GetPath(startCoordinate, endCoordinate, "foot-walking"));
                    return result;
                }

                Path ridingData = await GetPath(startStation.position.ToGeoCoordinate(),
                    endStation.position.ToGeoCoordinate(), "cycling-regular");
                Path fullWalkData = await GetPath(startCoordinate, endCoordinate, "foot-walking");
                if (NeedBike(startStation.route, ridingData, endStation.route, fullWalkData))
                {
                    Console.WriteLine("Bike is a good choice for this itinerary.");
                    Console.WriteLine("Returning riding itinerary.");
                    result.AddRoute(startStation.route);
                    result.AddRoute(ridingData);
                    result.AddRoute(endStation.route);
                }
                else
                {
                    Console.WriteLine("Bike is useless for this itinerary.");
                    Console.WriteLine("Returning riding itinerary.");
                    result.AddRoute(fullWalkData);
                }
                return result;
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("-------------- ERROR : TOO MANY REQUESTS --------------");
                result.message =
                    "Too many requests have been done on OpenRouteService API. Please try again in a moment !";
            }

            result.hasError = true;
            return result;
        }

        private bool NeedBike(Path start, Path ride, Path end, Path full)
        {
            double startTime = start.properties.summary.duration;
            double rideTime = ride.properties.summary.duration;
            double endTime = end.properties.summary.duration;

            return full.properties.summary.duration > startTime + rideTime + endTime;
        }

        private async Task<Station> GetClosestStation(CustomGeoCoordinate coordinate, bool start = true)
        {
            Console.WriteLine("Looking for the closest station from the point : " + coordinate);
            Station[] stations = (await GetStations())
                .OrderBy(s => coordinate.GetDistanceTo(s.position.ToGeoCoordinate())).ToArray();
            for (int i = 0; i < stations.Length; i += 5)
            {
                Station chosenStation = null;
                double dist = double.MaxValue;
                for (int j = i; j < i + 5; j++)
                {
                    if (j >= stations.Length) break;
                    Station s = stations[j];

                    if (s.contractName == "jcdecauxbike") continue;
                    Station station = await GetStation(s.number + "_" + s.contractName);
                    if (start
                            ? station.totalStands.availabilities.bikes <= 0
                            : station.totalStands.capacity - station.totalStands.availabilities.bikes <= 0) continue;
                    Path route = await GetPath(coordinate, station.position.ToGeoCoordinate(), "foot-walking");
                    if (route.properties.summary.distance > dist)
                        continue;
                    chosenStation = station;
                    chosenStation.route = route;
                    dist = route.properties.summary.distance;
                }

                if (chosenStation == null) continue;
                Console.WriteLine("Chosen station : " + chosenStation);
                return chosenStation;
            }

            Console.WriteLine("Did not find any station");
            return null;
        }
    }
}