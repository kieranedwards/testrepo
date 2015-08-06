using NUnit.Framework;
using PureRide.Web;

[SetUpFixture]
// ReSharper disable once CheckNamespace as SetUpFixture needs to be in global space
public class AutoMapperSetup
{
    [SetUp]
    public void Setup()
    {
        MappingConfiguration.ConfigureMappings();
    }

}
