using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Exerp.Api.Interfaces.Services;
using NUnit.Framework;

namespace Exerp.API.Integration
{
    [TestFixture]
    public class ClipPackPurchaseServiceTests
    {

        private IClipCardPurchaseService _purchaseService;
        [TestFixtureSetUp]
        public void Init()
        {
            using (var scope = DependencyConfiguration.RegisterDependencies().BeginLifetimeScope())
            {
                _purchaseService = scope.Resolve<IClipCardPurchaseService>();
            }
        }
  

        [Test]
        public void Get_credit_packs_for_region()
        {
            //Amount of credits
            //Cost
            //Title
            //Notes
            //Expiry

            //To include into offer

            //var result = _purchaseService.GetClipPacksForRegion("Scotland");        
           // Assert.IsNotNull(result);
        }

        [Test]
        public void Get_credit_packs_for_person_in_region()
        {
            //Amount of credits
            //Cost
            //Title
            //Notes
            //Expiry

            //Not include into offer
        }

        [Test]
        public void Get_credit_apply_promo()
        {
            //Amount of credits
            //Cost
            //Title
            //Notes
            //Expiry

            //Not include into offer
        }

        [Test]
        public void Purchase_CreditPacks()
        {

        }

    }
}
