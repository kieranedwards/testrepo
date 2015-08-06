using System;
using AutoMapper;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces.Services;
using PureRide.Web.ViewModels.Account;

namespace PureRide.Web.ApplicationServices.Account
{
    public class UpdatePersonDetailsModelBuilder : IUpdateDetailsModelBuilder
    {
        private readonly IPersonService _personService;
        private readonly IAuthenticationService _authenticationService;

        public UpdatePersonDetailsModelBuilder(IPersonService personService, IAuthenticationService authenticationService)
        {
            _personService = personService;
            _authenticationService = authenticationService;
        }

        public UpdateDetailsModel BuildModel()
        {
            var user = _authenticationService.GetCurrentUser();

            if(user==null)
                throw new NotSupportedException("User must be logged in");

            var details = _personService.GetPersonDetails(user);
            return Mapper.Map<UpdateDetailsModel>(details);
        }

        public void UpdatePerson(UpdateDetailsModel model)
        {
            var user = _authenticationService.GetCurrentUser();

            if (user == null)
                throw new NotSupportedException("User must be logged in");

            _personService.UpdatePerson(user, Mapper.Map<PersonDetails>(model));
        }
    }

    public interface IUpdateDetailsModelBuilder
    {
        UpdateDetailsModel BuildModel();
        void UpdatePerson(UpdateDetailsModel model);
    }

    
}
