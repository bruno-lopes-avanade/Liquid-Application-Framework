using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Liquid.Activation;
using Xunit;

namespace Liquid.OnAzure.Tests
{
    public class ServiceBusTest
    {
        private ServiceBus _sut;
        public ServiceBusTest()
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
}
