using Exerp.Api.WebServices.SubscriptionAPI;
using compositeKey = Exerp.Api.WebServices.SelfServiceAPI.compositeKey;
// ReSharper disable InconsistentNaming

namespace Exerp.Api.Interfaces.WebServices
{
    internal interface ISelfServiceApi
    {
        compositeKey findPersonByEmail(string emailAddress);
        bool validatePassword(compositeKey personKey, string password);
        bool updatePassword(compositeKey personKey, string currentPassword, string newPassword);
    }
}
