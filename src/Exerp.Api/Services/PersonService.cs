using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services.Protocols;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Configuration;
using Exerp.Api.Interfaces.Services;
using Exerp.Api.Interfaces.WebServices;
using Exerp.Api.WebServices.BookingAPI;
using Exerp.Api.WebServices.PersonAPI;
using compositeKey = Exerp.Api.WebServices.SelfServiceAPI.compositeKey;
using personType = Exerp.Api.WebServices.PersonAPI.personType;

namespace Exerp.Api.Services
{
    internal class PersonService : IPersonService
    {
        private readonly ISelfServiceApi _selfServiceApi;
        private readonly IPersonApi _personApi;
        private readonly ICentreService _centreService;
        private readonly ISubscriptionApi _subscriptionApi;
        private readonly ISocialApi _socialApi;
        private readonly IBookingApi _bookingApi;
        private readonly IExerpConfiguration _exerpConfiguration;

        public PersonService(ISelfServiceApi selfServiceApi, 
            IPersonApi personApi, 
            ICentreService centreService, 
            ISubscriptionApi subscriptionApi, 
            ISocialApi socialApi, 
            IBookingApi bookingApi,
            IExerpConfiguration exerpConfiguration)
        {
            _selfServiceApi = selfServiceApi;
            _personApi = personApi;
            _centreService = centreService;
            _subscriptionApi = subscriptionApi;
            _socialApi = socialApi;
            _bookingApi = bookingApi;
            _exerpConfiguration = exerpConfiguration;
        }

        public Identity ValidateLoginByEmailAddress(string emailAddress, string password)
        {
            personDetail details = null;
            try
            {
                details = _personApi.getPersonDetailByLogin(emailAddress, password);
            }
            catch (SoapException ex)
            {
                //ToDo:fix error handling
                //expected if the password is wrong
                Console.Write(ex.Detail);
            }

            return details!=null ? new Identity(details.person.personId.center, details.person.personId.id) : null;
        }

        public Identity ValidateLoginById(Identity personIdentity, string password)
        {
            var isValidUser = _selfServiceApi.validatePassword(Mapper.Map<compositeKey>(personIdentity), password);
            return isValidUser ? personIdentity : null;
        }

        public Identity CreatePersonWithDetails(PersonDetails personDetails)
        {
            var centreId = _exerpConfiguration.ExerpPureRidePrimaryCentreId;

           var result = _personApi.createPersonWithCommunicationDetails(centreId, new fullPersonAndCommunicationDetails()
                {
                    password = personDetails.Password,
                    person = new person()
                    {
                         firstName = personDetails.FirstName,
                         lastName = personDetails.LastName,
                         birthday = personDetails.Birthday.ToString(Constants.ApiDateFormat),
                         male = personDetails.IsMale,
                         personType = personType.PRIVATE,
                         personTypeSpecified = true,
                         address = new address()
                    },
                    personCommunication = new personCommunication()
                    {
                         mobilePhoneNumber = personDetails.MobilePhoneNumber,
                         email = personDetails.Email,
                         allowEmail = true,
                         allowEmailSpecified =true,
                         allowSMS = personDetails.AllowSms,
                         allowSMSSpecified = true,
                         wishToReceiveThirdPartyOffers = personDetails.WishToReceiveThirdPartyOffers,                     
                    }
                });

            var personId = new Identity(centreId, result.person.personId.id);

            _personApi.updateExtendedAttributeText(new personKey { center = personId.CentreId, id = personId.EntityId }, Constants.ShoeSizeEaKey, personDetails.ShoeSize);
            _personApi.updateExtendedAttributeText(new personKey { center = personId.CentreId, id = personId.EntityId }, Constants.HealthEaKey, "True");
            _personApi.updateExtendedAttributeText(new personKey { center = personId.CentreId, id = personId.EntityId }, Constants.TermsEaKey, "True");

           return personId;
        }

        public Identity CreatePersonFriend(Person personDetails)
        {
            var centreId = _exerpConfiguration.ExerpPureRidePrimaryCentreId;

            var result = _personApi.createPersonWithCommunicationDetails(centreId, new fullPersonAndCommunicationDetails()
            {
                person = new person()
                {
                    firstName = personDetails.FirstName,
                    lastName = personDetails.LastName,
                    personType = personType.PRIVATE,
                    personTypeSpecified = true,
                    birthday = (new DateTime(1970,1,1)).ToString(Constants.ApiDateFormat),
                    address = new address()
                },
                personCommunication = new personCommunication()
                {
                    mobilePhoneNumber = personDetails.MobilePhoneNumber,
                    email = personDetails.Email,
                    allowEmail = (string.IsNullOrWhiteSpace(personDetails.Email)),
                    allowEmailSpecified = true,
                    allowSMS = (string.IsNullOrWhiteSpace(personDetails.MobilePhoneNumber)),
                    allowSMSSpecified = true,
                    wishToReceiveThirdPartyOffers = false,
                }
            });

            return new Identity(centreId, result.person.personId.id);
        }

        public void CreatePersonFriendLink(Identity person1Id, Identity person2Id)
        {
            _socialApi.createFriendRelation(Mapper.Map<WebServices.SocialAPI.compositeKey>(person1Id), Mapper.Map<WebServices.SocialAPI.compositeKey>(person2Id));
        }

        public Identity UpgradeFriendPersonWithDetails(Identity personId, PersonDetails personDetails, string token)
        {
            _personApi.updateDetails(new person()
            {
                personId = Mapper.Map<WebServices.PersonAPI.compositeKey>(personId),
                firstName = personDetails.FirstName,
                lastName = personDetails.LastName,
                birthday = personDetails.Birthday.ToString(Constants.ApiDateFormat),
                male = personDetails.IsMale,
                address = new address()
            });

            _personApi.updateCommunicationDetails(new personCommunication()
            {
                personId = Mapper.Map<WebServices.PersonAPI.compositeKey>(personId),
                mobilePhoneNumber = personDetails.MobilePhoneNumber,
                email = personDetails.Email,
                allowEmail = true,
                allowEmailSpecified = true,
                allowSMS = personDetails.AllowSms,
                allowSMSSpecified = true,
                wishToReceiveThirdPartyOffers = personDetails.WishToReceiveThirdPartyOffers,
            });

            UpdatePersonPasswordWithToken(personDetails.Password, token, personId);
            
            _personApi.updateExtendedAttributeText(new personKey { center = personId.CentreId, id = personId.EntityId }, Constants.ShoeSizeEaKey, personDetails.ShoeSize);
            _personApi.updateExtendedAttributeText(new personKey { center = personId.CentreId, id = personId.EntityId }, Constants.HealthEaKey, "True");
            _personApi.updateExtendedAttributeText(new personKey { center = personId.CentreId, id = personId.EntityId }, Constants.TermsEaKey, "True");

            return personId;
        }


        public bool UpdatePersonPassword(Identity personIdentity,string currentPassword, string newPassword)
        {
            return _selfServiceApi.updatePassword(Mapper.Map<compositeKey>(personIdentity), currentPassword, newPassword);
        }
         
        public bool UpdatePersonPasswordWithToken(string newPassword, string token, Identity personId)
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;
            
            try
            {
                _personApi.changePasswordWithToken(Mapper.Map<personKey>(personId), newPassword, token);
            }
            catch (Exception ex)
            {
                Console.Write(ex);//todo: add tracking
            }
            
            return true;
        }

        public bool RequestPersonPasswordReset(string emailAddress)
        {
            var findResult = this.FindPersonByEmailAddress(emailAddress);

            if (findResult.FoundState != FindPersonStatus.FullAccount)
                return false;

            _personApi.sendPasswordToken(Mapper.Map<personKey>(findResult.PersonId)); 
            return true;
        }

        public void UpdatePerson(Identity personId, PersonDetails personDetails)
        {

            _personApi.updateDetails(new person()
            {
                personId = Mapper.Map<WebServices.PersonAPI.compositeKey>(personId),
                firstName = personDetails.FirstName,
                lastName = personDetails.LastName,
                birthday = personDetails.Birthday.ToString(Constants.ApiDateFormat),
                male = personDetails.IsMale
            });

            _personApi.updateCommunicationDetails(new personCommunication()
            {
                personId = Mapper.Map<WebServices.PersonAPI.compositeKey>(personId),
                mobilePhoneNumber = personDetails.MobilePhoneNumber,
                email = personDetails.Email,
                allowSMS = personDetails.AllowSms,
                allowSMSSpecified = true,
                wishToReceiveThirdPartyOffers = personDetails.WishToReceiveThirdPartyOffers,
            });

            _personApi.updateExtendedAttributeText(new personKey { center = personId.CentreId, id = personId.EntityId }, Constants.ShoeSizeEaKey, personDetails.ShoeSize);
        }

        public PersonDetails GetPersonDetails(Identity personId)
        {
           var result = _personApi.getPersonDetail( Mapper.Map<personKey>(personId));
            return Mapper.Map<PersonDetails>(result);
        }
        
        private static bool PersonFound(compositeKey person)
        {
            return person != null && person.id > 0;
        }
     
        public void UpdatePersonAddress(Identity personId, Address address)
        {
            if (personId == null)
                throw new ArgumentNullException("personId","Person cannot be null");

            _personApi.updateDetails(new person()
            {
                address = Mapper.Map<address>(address),
                personId = Mapper.Map<personKey>(personId)
            });
        }

        public FindPersonResult FindPersonByEmailAddress(string emailAddress)
        {
            var result = new FindPersonResult();

            var personId = _selfServiceApi.findPersonByEmail(emailAddress);
            if (!PersonFound(personId))
                return result;

            //look up they have ticked Terms
            var details = _personApi.getPersonDetail(new personKey() {center = personId.center, id = personId.id});
            var termsHaveBeenAgreed = details.extendedAttributes.Any(a => a.id == Constants.TermsEaKey && a.value == "true");

            result.FoundState = termsHaveBeenAgreed ? FindPersonStatus.FullAccount : FindPersonStatus.FriendOnly;
            result.PersonId = Mapper.Map<Identity>(personId);

            return result;
        }

        public IEnumerable<PurchasedClipCard> GetActiveClipCards(Identity personId)
        {
            return GetClipCards(personId,false);
        }

        public IEnumerable<PurchasedClipCard> GetInActiveClipCards(Identity personId)
        {
            return GetClipCards(personId, true);
        }

        private IEnumerable<PurchasedClipCard> GetClipCards(Identity personId,bool onlyFinished)
        {
            if (personId == null)
                return Enumerable.Empty<PurchasedClipCard>();

            var result = _subscriptionApi.getPersonClipCards(new WebServices.SubscriptionAPI.compositeKey() { center = personId.CentreId, id = personId.EntityId }, onlyFinished, false, false);
            return result.Select(a => { 
                                          var card = Mapper.Map<PurchasedClipCard>(a);
                                          var centre = _centreService.GetActiveCentreById(a.clipcardId.center);
                                          card.Region = centre==null ? string.Empty : centre.Region;
                                          return card;
            }).ToArray();
        }

       
        public Dictionary<int, IEnumerable<Participation>> GetFutureParticipations(Identity personId)
        {
            var user = Mapper.Map<WebServices.BookingAPI.compositeKey>(personId);

            return GroupParticipationsByBookingId(
                _bookingApi.findPersonParticipations(user)
                .Where(a =>a.state == participationState.BOOKED || a.state == participationState.OVERBOOKED_WAITINGLIST));
            
        }

        public  Dictionary<int, IEnumerable<Participation>> GetPastParticipations(Identity personId)
        {
            var user = Mapper.Map<WebServices.BookingAPI.compositeKey>(personId);
            
            var nonShowUp = _bookingApi.getPersonParticipations(user,
                DateTime.Now.AddMonths(-6).ToString(Constants.ApiDateFormat),
                DateTime.Now.ToString(Constants.ApiDateFormat), 
                personalBookingType.SHOW_UPS);

            var showUp = _bookingApi.getPersonParticipations(user, 
                DateTime.Now.AddMonths(-6).ToString(Constants.ApiDateFormat), 
                DateTime.Now.ToString(Constants.ApiDateFormat), 
                personalBookingType.NON_SHOW_UPS);

            return GroupParticipationsByBookingId(showUp.Concat(nonShowUp));
        }

        private Dictionary<int, IEnumerable<Participation>> GroupParticipationsByBookingId(IEnumerable<participation> participations)
        {
            return Enumerable.ToDictionary((from p in participations
                        group p by p.booking.bookingId.id into g
                        select new { g.Key, Value = (g.Select(Mapper.Map<Participation>)) }), a => a.Key, b => b.Value);
        }

    }
}
