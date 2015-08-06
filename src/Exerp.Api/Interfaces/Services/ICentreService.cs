using System.Collections.Generic;
using Exerp.Api.DataTransfer;

namespace Exerp.Api.Interfaces.Services
{
    public interface ICentreService
    {
        Centre GetActiveCentreById(int centreId);
        Centre GetActiveCentreByName(string centreName);
        IEnumerable<string> GetActiveRegions();
        
        /// <summary>
        /// Centre Name and Details
        /// </summary>
        Dictionary<string, Centre> GetActiveCentresWithDetails();

        /// <summary>
        /// Centre ID and Name
        /// </summary>
        Dictionary<int, string> GetActiveCentres();
   }
}