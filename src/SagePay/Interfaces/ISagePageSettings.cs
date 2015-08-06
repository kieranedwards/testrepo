namespace SagePay.Interfaces
{
    public interface ISagePageSettings
    {
        string SagePayEndPoint { get;  }
        string SagePayNotificationUrl { get; }
        string SagePayVendorName { get; }
        string SagePayReturnUrl { get;}
    }
}