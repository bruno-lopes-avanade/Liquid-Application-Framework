using System;
using Xunit;

namespace Liquid.OnAzure.IntegrationTests
{
    public class ServiceBusIntegrationTest
    {
        private ServiceBus _sut;
        public ServiceBusIntegrationTest()
        {
            //Workbench.Instance.Configuration["ServiceBus"] = "Endpoint=sb://liquid-sample.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=bFsoGSUAK5/mKzpk4NOEhTA0n9blvApF8TTCJN23yP0=";
            _sut = new ServiceBus(new ServiceBusConfiguration
            {
                ConnectionString = "Endpoint=sb://liquid-sample.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=bFsoGSUAK5/mKzpk4NOEhTA0n9blvApF8TTCJN23yP0=",
            });
        }

        [Fact]
        public void ShouldThrowExceptionWhenConfigurationIsNull()
        {
            Assert.ThrowsAny<Exception>(() => _sut.ProcessQueue());
        }
    }

    public class MessageBusUnderTest
    {
    }

}
