using System.Globalization;
using Umbraco.Core.Models;

namespace PureRide.Web.ViewModels.Location
{
    public class LocationModel
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Region { get; set; }
    }
}