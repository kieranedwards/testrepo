using AutoMapper;
using NUnit.Framework;

namespace Exerp.Api.Tests
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
