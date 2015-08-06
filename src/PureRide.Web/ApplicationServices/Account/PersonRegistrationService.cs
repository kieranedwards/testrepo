using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using log4net.Util;
using PureRide.Web.ViewModels.Account;

namespace PureRide.Web.ApplicationServices.Account
{
    public class PersonRegistrationService : IPersonRegistrationService
    {
        private readonly IPersonService _personService;
        private readonly IAuthenticationService _authenticationService;

        public PersonRegistrationService(IPersonService personService, IAuthenticationService authenticationService)
        {
            _personService = personService;
            _authenticationService = authenticationService;
        }

        public Identity RegisterPerson(RegisterPersonModel model)
        {
            var person = Mapper.Map<PersonDetails>(model);

            var findResult = _personService.FindPersonByEmailAddress(model.Email);
            if (findResult.FoundState==FindPersonStatus.FullAccount)
                return null;

            Identity result;

            if(findResult.FoundState == FindPersonStatus.FriendOnly)
                result = _personService.UpgradeFriendPersonWithDetails(findResult.PersonId, person, model.Token);
            else
                result = _personService.CreatePersonWithDetails(person);    
            
            _authenticationService.LoginPerson(result);

            return result;
        }

      

        public Identity RegisterFriend(RegisterFriendModel model)
        {
            var person = Mapper.Map<Person>(model);
            Identity personId;

            FindPersonResult findResult = new FindPersonResult();

            if (EmailAddressProvided(model))
            {
                findResult = _personService.FindPersonByEmailAddress(model.Email); 
            }

            personId = findResult.FoundState == FindPersonStatus.NotFound ? _personService.CreatePersonFriend(person) : findResult.PersonId;

            if (PersonIsCurrentUser(personId))
            {
                return null;
            }

            _personService.CreatePersonFriendLink(_authenticationService.GetCurrentUser(), personId);
             
            return personId;
        }

        private bool PersonIsCurrentUser(Identity personId)
        {
            return personId.Equals(_authenticationService.GetCurrentUser());
        }

        public bool PreRegisterPerson(AccountEmailModel model)
        {
            var findResult = _personService.FindPersonByEmailAddress(model.Email);
            
            if (findResult.FoundState != FindPersonStatus.NotFound)
            {
                _personService.RequestPersonPasswordReset(model.Email);
            }

            return findResult.FoundState == FindPersonStatus.NotFound;
        }

        public RegisterPersonModel BuildRegisterForm(string email, string token, string pid)
        {
            var result = new RegisterPersonModel
            {
                Pid = pid,
                Token = token,
                Email = email
            };

            if (!string.IsNullOrWhiteSpace(pid))
            {
                var details = _personService.GetPersonDetails(new Identity(pid));
                result.Email = details.Email;
                result.MobilePhoneNumber = details.MobilePhoneNumber;
                result.FirstName = details.FirstName;
                result.LastName = details.LastName;
            }

            return result;
        }
        
        private static bool EmailAddressProvided(RegisterFriendModel model)
        {
            return !string.IsNullOrWhiteSpace(model.Email);
        }
    }

    public interface IPersonRegistrationService
    {
        Identity RegisterPerson(RegisterPersonModel model);
        Identity RegisterFriend(RegisterFriendModel model);

        bool PreRegisterPerson(AccountEmailModel model);
        RegisterPersonModel BuildRegisterForm(string email, string token, string pid);
    }
}
