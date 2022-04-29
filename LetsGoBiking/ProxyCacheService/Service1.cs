using System.Threading.Tasks;

namespace ProxyCacheService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public Task<string> GetStation(string stationId)
        {
            return JCDecaux.GetStationAsync(stationId);
        }
        
        public Task<string> GetStations()
        {
            return JCDecaux.GetStationAsync("all");
        }
    }
}
