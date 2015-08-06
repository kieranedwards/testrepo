using Exerp.Api.WebServices.SocialAPI;
// ReSharper disable InconsistentNaming

namespace Exerp.Api.Interfaces.WebServices
{
    internal interface ISocialApi
    {
        void createFriendRelation(compositeKey personId, compositeKey person2Id);
    }
}
