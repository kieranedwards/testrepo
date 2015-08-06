using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Exerp.Api;
using Exerp.Api.DataTransfer;
using Exerp.Api.Interfaces;
using Exerp.Api.Interfaces.Services;
using NUnit.Framework;

namespace Exerp.API.Integration
{
    [TestFixture]
    public class PersonTests
    {
        private IPersonService _personService;

        [TestFixtureSetUp]
        public void Init()
        {
            using (var scope = DependencyConfiguration.RegisterDependencies().BeginLifetimeScope())
            {
                _personService = scope.Resolve<IPersonService>();
            }
        }

        [Test]
        public void GetBookedClasses()
        {
        }

        [Test]
        public void GetWaitingListClasses()
        {
        }

        [Test]
        public void ValidateLoginByEmailAddressWhenValid()
        {
          /*  var config = new Moq
            var result = _personService.ValidateLoginByEmailAddress("john.kilmister@puregym.com", config.ApiCredentials.Password);
            
            Assert.IsNotNull(result);
            Assert.That(result.CentreId > 0);
            Assert.That(result.EntityId > 0);
            */
        }  

        [Test]
        public void ValidateLoginByEmailAddressWhenInValid()
        {
            var result = _personService.ValidateLoginByEmailAddress("test@test.com", "");

            Assert.IsNull(result);
        }

        [Test]
        public void CreatePersonWithDetails()
        {
        }

        [Test]
        public void UpdatePersonWithDetails()
        {
        }

        [Test]
        public void UpdatePersonPassword()
        {
        }

    }
}
