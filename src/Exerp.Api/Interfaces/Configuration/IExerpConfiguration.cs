using System.Net;

namespace Exerp.Api.Interfaces.Configuration
{
    public interface IExerpConfiguration
    {
        string ExerpApiBaseUrl { get; }
        NetworkCredential ExerpApiCredentials { get; }
        int ExerpPureRideScopeId { get; }
        int ExerpPureRidePrimaryCentreId { get;  }
    }
}
