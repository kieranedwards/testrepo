using System;

namespace PureRide.Web.Exceptions
{
    public class BasketNotFoundException : Exception
    {
        public BasketNotFoundException(string vpsTxId)
            : base(string.Concat("Basket Could not be loaded with sagepay transaction ref: ", vpsTxId))
        {
        }
    }

}