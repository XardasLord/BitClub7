using BC7.Business.Implementation.Tests.Integration.Base;
using NUnit.Framework;

namespace BC7.Business.Implementation.Tests.Integration.Tests.AutoMapper
{
    public class AutoMapperConfigurationTest : BaseIntegration
    {
        [Test]
        public void AutoMapperConfigurationValidation()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
