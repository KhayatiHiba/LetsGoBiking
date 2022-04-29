using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RoutingService.ProxyService;

namespace RoutingService
{
    internal static class RequestHelper
    {
        private static readonly HttpClient client = new();
        private static readonly Service1Client proxyClient = new();
        private const string URL = "https://api.openrouteservice.org/";
        private const string API_KEY = "5b3ce3597851110001cf62480a82a61d8305465f89bfc1163a0951d4";

        internal static async Task<Station> GetStation(string id)
        {
            return JsonConvert.DeserializeObject<Station>(await proxyClient.GetStationAsync(id));
        }
    
        internal static async Task<Station[]> GetStations()
        {
            return JsonConvert.DeserializeObject<Station[]>(await proxyClient.GetStationsAsync());
        }

        /**
         * 
         **/
        internal static async Task<Path> GetPath(CustomGeoCoordinate coordinate, CustomGeoCoordinate stationPos,
            string meansOfTransportation)
        {
            HttpResponseMessage response = await client.GetAsync(
                URL + "v2/directions/" + meansOfTransportation + "?api_key=" + API_KEY + "&start=" + coordinate +
                "&end=" + stationPos);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            Response resp = JsonConvert.DeserializeObject<Response>(responseBody);
            return new Path(resp!.features[0].properties, resp.features[0].geometry);
        }

        internal static async Task<Coordinate> GetCoordinate(string location)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(URL + "geocode/search?api_key=" + API_KEY + "&text=" + location);
                response.EnsureSuccessStatusCode();
                return new Coordinate(JsonConvert.DeserializeObject<JObject>(
                        await response.Content.ReadAsStringAsync())["features"][0]["geometry"]!["coordinates"]
                    .ToObject<double[]>());
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }
    }
}