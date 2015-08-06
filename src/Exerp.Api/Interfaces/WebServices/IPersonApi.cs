using Exerp.Api.WebServices.PersonAPI;
// ReSharper disable InconsistentNaming due to Java code

namespace Exerp.Api.Interfaces.WebServices
{
    internal interface IPersonApi
    {
        centerDetail[] getDetailForCenters(string countryName);
        scope getScope(scopeType scopeType, int scopeId);
        fullPersonAndCommunicationDetails createPersonWithCommunicationDetails(int centerId,
        fullPersonAndCommunicationDetails fullPersonCommunication);
        void sendPasswordToken(personKey personKey);
        void changePasswordWithToken(personKey personKey, string newPassword, string token);

        person getDetails(personKey personKey);
        personDetail getPersonDetail(personKey personKey);
        person updateDetails(person person);
        void updateCommunicationDetails(personCommunication personCommunication);
        void updateExtendedAttributeText(personKey personKey, string key, string value);
        personDetail getPersonDetailByLogin(string email, string password);
    }
}
