using AutoMapper;
using NUnit.Framework;
using PureRide.Web;

namespace Web.Tests
{
    [TestFixture]
    public class MappingConfigurationTests
    {
        [Test]
        public void When_ConfigureMappings_EnsureValid()
        {
            MappingConfiguration.ConfigureMappings();
            Mapper.AssertConfigurationIsValid();
        }

    }
}
