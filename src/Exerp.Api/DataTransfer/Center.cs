using System;

namespace Exerp.Api.DataTransfer
{
    /// <remarks>
    /// Note this external class is UK Spelling which does not match the API
    /// </remarks>
    public class Centre
    {
        public int CenterId { get; set; }//todo fix mapper to correct spelling
        public string WebName { get; set; }
        public string Email { get; set; }
        public string Region { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Postcode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
