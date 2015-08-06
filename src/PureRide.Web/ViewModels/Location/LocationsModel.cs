using System.Collections.Generic;
using System.Linq;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace PureRide.Web.ViewModels.Location
{
    public class LocationsModel 
    {
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<LocationModel> Locations { get; set; }
        public string SelectedRegion { get; set; }

        public LocationsModel()
        {
            Regions = Enumerable.Empty<string>();
            Locations = Enumerable.Empty<LocationModel>();
        }
    }
}
