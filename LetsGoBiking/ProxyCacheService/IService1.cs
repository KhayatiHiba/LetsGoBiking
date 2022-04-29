using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ProxyCacheService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        // Get the station from the database 
        Task<string> GetStation(string stationId);

        [OperationContract]
        //Get all stations from the database and return them as a string in JSON format with an async Task
        Task<string> GetStations();
    }

}