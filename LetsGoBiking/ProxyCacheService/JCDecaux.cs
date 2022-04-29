using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace ProxyCacheService
{
    public class JCDecaux
    {
        private static readonly HttpClient Client = new HttpClient();
        private const string Url = "https://api.jcdecaux.com/vls/v3/";
        private const string ApiKey = "1d12a78f891ef602f3167019561a3c0eec784ecc";
        
        private static readonly Cache<string> Cache = new Cache<string>(RetrieveDataAsync);
        private static async Task<string> RetrieveDataAsync(string cacheItemName)
        {
            Console.WriteLine("Requesting informations about station with key : " + cacheItemName);
            if (cacheItemName == "all")
            {
                return await Client.GetStringAsync(Url + "stations?apiKey=" + ApiKey);
            }
            var val = cacheItemName.Split('_');
            return await Client.GetStringAsync(Url + "/stations/" + val[0] + "?contract=" + val[1] + "&apiKey=" + ApiKey);
        }
            
            public static async Task<string> GetStationAsync(string nameOfItem) => await Cache.Get(nameOfItem);
    }
}