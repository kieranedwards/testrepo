using Exerp.Api.WebServices.SubscriptionAPI;

namespace Exerp.Api.DataTransfer
{
    public class AvailableClipcard
    {
        public int Clips { get; set; }
        
        public string Name { get; set; }

        public string NormalPrice { get; set; }

        public string Price { get; set; }

        public Identity ProductId;

        public int ValidPeriodLength { get; set; }

        public bool ValidPeriodLengthSpecified { get; set; }

        public timeUnit ValidPeriodLengthUnit{ get; set; }

        public bool ValidPeriodLengthUnitSpecified{ get; set; }

    }
}
