using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Helpers;
using Exerp.Api.Interfaces.Configuration;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.WebServices.PersonAPI;

namespace Exerp.Api.Services
{
    internal class CentreService : ICentreService
    {
        private readonly IPersonApi _personApi;
        private readonly IExerpConfiguration _exerpConfiguration;

        public CentreService(IPersonApi personApi, IExerpConfiguration exerpConfiguration)
        {
            _personApi = personApi;
            _exerpConfiguration = exerpConfiguration;
        }

        public Dictionary<string, Centre> GetActiveCentresWithDetails()
        {
            var pureRideCentres = GetPureRideCenterIdsAndRegions();
            return GetCentreDetailsWithRegion(pureRideCentres);
        }

        public IEnumerable<string> GetActiveRegions()
        {
            return GetActiveCentresWithDetails().Values.Select(a => a.Region).Distinct();
        }

        public Centre GetActiveCentreById(int center)
        {
            throw new NotSupportedException("Use the cached implementation");
        }

        public Centre GetActiveCentreByName(string centreName)
        {
            var nameAsKey = centreName.ToUpper();
            var centres = GetActiveCentresWithDetails();
            return centres.ContainsKey(nameAsKey) ? centres[nameAsKey] : null;
        }

        public Dictionary<int, string> GetActiveCentres()
        {
            var apiResult = _personApi.getDetailForCenters(String.Empty);
            return apiResult.Where(a => IsOpenPureRideCentre(a.startupDate)).Select(a => new { id = a.centerId, name = a.webName }).ToDictionary(a => a.id, b => b.name);
        }

        private Dictionary<string, Centre> GetCentreDetailsWithRegion(Dictionary<int, string> pureRideCentres)
        {
            var results = new Dictionary<string, Centre>();
            var apiResult = _personApi.getDetailForCenters(String.Empty);

            foreach (var center in apiResult)
            {
                if (!IsValidPureRideCentre(pureRideCentres, center)) 
                    continue;

                var centerDto = Mapper.Map<Centre>(center);
                centerDto.Region = pureRideCentres[center.centerId];

                results.Add(centerDto.WebName.ToUpper(), centerDto);
            }
            return results;
        }

        private static bool IsValidPureRideCentre(Dictionary<int, string> pureRideCentres, centerDetail center)
        {
            return pureRideCentres.ContainsKey(center.centerId) && IsOpenPureRideCentre(center.startupDate);
        }

        private static bool IsOpenPureRideCentre(string centerOpenDate)
        {
            DateTime openDate = centerOpenDate.FromApiDateString();
            var isValidOpenDate = openDate != DateTime.MinValue;
            return (isValidOpenDate && openDate <= DateTime.Now);
        }

        private Dictionary<int, string> GetPureRideCenterIdsAndRegions()
        {
            var resultTree = _personApi.getScope(scopeType.Area, _exerpConfiguration.ExerpPureRideScopeId);
            var areas = resultTree.children.Single(b => b.name == "United Kingdom").children;

            var pureRideCentres = new Dictionary<int, string>();

            foreach (var area in areas)
            {
                foreach (var center in area.children)
                {
                    ValidateCentreId(center.id);
                    pureRideCentres.Add(center.id, area.name);
                }
            }
            return pureRideCentres;
        }

        private void ValidateCentreId(int centreId)
        {
            if (centreId <= 700)
                throw new ArgumentOutOfRangeException("centreId", "PureRide Centres can only have an Id of > 700");
        }
         
    }
}
