using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Offline.SampleData;
using Exerp.Api.Interfaces.Services;

namespace Exerp.Api.Offline.Services
{

    public class CentreService : ICentreService
    {
        private readonly ISampleDataProvider _sampleDataProvider;

        public CentreService(ISampleDataProvider sampleDataProvider)
        {
            _sampleDataProvider = sampleDataProvider;
        }

        public Centre GetActiveCentreById(int centreId)
        {
            throw new NotImplementedException();
        }

        public Centre GetActiveCentreByName(string centreName)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<string> GetActiveRegions()
        {
            var data = ((IEnumerable<dynamic>)_sampleDataProvider.GetDataSet().centres);
            return data.Select(a=>(string)a.region).Distinct();
        }

        public Dictionary<string, Centre> GetActiveCentresWithDetails()
        {
        /*    var data = ((IEnumerable<dynamic>)_sampleDataProvider.GetDataSet().centres);
            return data.Select(a => new Centre()
            {
                CenterId = a.id,
                WebName = a.name,
                Email = a.email,
                Region = a.region,
                Longitude = a.longitude,
                Latitude = a.latitude,
                AddressLine1 = a.addressLine1,
                AddressLine2 = a.addressLine2,
                AddressLine3 = a.addressLine3,
                Postcode = a.postcode,
                PhoneNumber = a.phoneNumber,
            });/*
         */
            return new Dictionary<string, Centre>();
        }

        public Dictionary<int, string> GetActiveCentres()
        {
            throw new NotImplementedException();
        }


        public bool IsActiveCentreName(string centreName)
        {
            //var data = GetActiveCentresWithDetails();
            //return data.Any(a => string.Compare(a.WebName, centreName, StringComparison.OrdinalIgnoreCase) == 0);
            throw new NotImplementedException();
        }

    }
}
