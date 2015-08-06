using System;
using System.Web.Services.Protocols;
using Exerp.Api.Interfaces.Configuration;

namespace Exerp.Api.Helpers
{
    internal static class WebServiceExtensions
    {
        public static T WithConfig<T>(this T service, IExerpConfiguration config) where T : SoapHttpClientProtocol
        {
            service.Credentials = config.ExerpApiCredentials; 
            service.Url = string.Concat(config.ExerpApiBaseUrl, service.GetType().ToString().GetServiceName());
            return service;
        }

        private static string GetServiceName(this string input)
        {
            var className = input.Substring(input.LastIndexOf(".", StringComparison.Ordinal)+1);
            return className.Substring(0, className.Length - "Service".Length);
        }
    }
}
