using System;
using Exerp.Api.WebServices.SubscriptionAPI;

namespace Exerp.Api.DataTransfer
{
    public class PurchasedClipCard
    {
        public bool Blocked { get; set; }

        public bool Cancelled{ get; set; }

        public compositeSubKey ClipcardId{ get; set; }

        public string ClipcardTypeName{ get; set; }

        public int ClipsInitial{ get; set; }

        public int ClipsLeft{ get; set; }

        public bool Finished{ get; set; }

        public compositeSubKey InvoiceLineId{ get; set; }

        public compositeKey MainSubscriptionId{ get; set; }

        public compositeKey PersonId{ get; set; }

        public DateTime ValidUntilDate{ get; set; }

        public string Region { get; set; }
    }
}