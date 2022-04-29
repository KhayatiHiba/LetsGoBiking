using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace RoutingService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
            UriTemplate = "ComputePath?location={location}&destination={destination}",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare)]
        Task<Result> ComputePath(string location, string destination);
    }
}
