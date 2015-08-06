using System.Linq;
using AutoMapper;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ViewModels.Location;

namespace PureRide.Web.ApplicationServices.Location
{
    public class LocationViewModelBuilder : ILocationViewModelBuilder
    {
        private readonly ICentreService _centreService;

        public LocationViewModelBuilder(ICentreService centreService)
        {
            _centreService = centreService;
        }

        public LocationsModel BuildModelForAll()
        {
            var activeRegions = _centreService.GetActiveRegions().ToArray();

            var locations = _centreService.GetActiveCentresWithDetails().Values.Select(Mapper.Map<LocationModel>);
    
            return new LocationsModel() { Locations = locations, Regions = activeRegions };
        }

        public LocationModel BuildModelForLocation(string locationName)
        {
            var location = _centreService.GetActiveCentreByName(locationName);
            return location == null ? null : Mapper.Map<LocationModel>(location);
        }
    }

    public interface ILocationViewModelBuilder
    {
        LocationsModel BuildModelForAll();
        LocationModel BuildModelForLocation(string locationName);
    }
 
}
