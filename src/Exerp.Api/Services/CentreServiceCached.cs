using System;
using System.Collections.Generic;
using System.Linq;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureGym.Core.Interfaces;
// ReSharper disable InconsistentlySynchronizedField

namespace Exerp.Api.Services
{
    internal class CentreServiceCached : ICentreService
    {
        private readonly ICentreService _centreService;
        private readonly ICacheProvider _cacheProvider;
        private const string CacheKeyCentres = "PureRideCentres";
        private const string CacheKeyById = "PureRideCentresById";
        private const int CacheExpiryHours = 12;
        private readonly object _locker = new object();

        public CentreServiceCached(ICentreService centreService, ICacheProvider cacheProvider)
        {
            _centreService = centreService;
            _cacheProvider = cacheProvider;
        }

        public Centre GetActiveCentreById(int centreId)
        {
            var centres = GetActiveCentres();
            
            if (!centres.ContainsKey(centreId)) 
                return null;

            var centreName = centres[centreId];
            return this.GetActiveCentreByName(centreName);
        }

        public Centre GetActiveCentreByName(string centreName)
        {
            var nameAsKey = centreName.ToUpper();
            var centres = GetActiveCentresWithDetails();
            return centres.ContainsKey(nameAsKey) ? centres[nameAsKey] : null;
        }
        
        public IEnumerable<string> GetActiveRegions()
        {
            return GetActiveCentresWithDetails().Values.Select(a => a.Region).Distinct();
        }

        public Dictionary<string, Centre> GetActiveCentresWithDetails()
        {
            var allCentres = _cacheProvider.GetValue<Dictionary<string, Centre>>(CacheKeyCentres);

            if (allCentres == null)
            {
                lock (_locker)
                {
                    allCentres = _cacheProvider.GetValue<Dictionary<string, Centre>>(CacheKeyCentres);
                    if (allCentres == null)
                    {
                        allCentres = _centreService.GetActiveCentresWithDetails();
                        _cacheProvider.Add(CacheKeyCentres, allCentres, new TimeSpan(CacheExpiryHours, 0, 0));
                    }
                }
            }

            return allCentres;
        }

        public Dictionary<int, string> GetActiveCentres()
        {
            var allCentres = _cacheProvider.GetValue<Dictionary<int, string>>(CacheKeyById);

            if (allCentres == null)
            {
                lock (_locker)
                {
                    allCentres = _cacheProvider.GetValue<Dictionary<int, string>>(CacheKeyById);
                    if (allCentres == null)
                    {
                        allCentres = _centreService.GetActiveCentres();
                        _cacheProvider.Add(CacheKeyById, allCentres, new TimeSpan(CacheExpiryHours, 0, 0));
                    }
                }
            }

            return allCentres;
        }
    }
}
