using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Reporting.Test
{
    public class TestBase
    {
        protected readonly ILoggerFactory _logFactory ;
        protected readonly INodeServices _nodeServices;
        public TestBase()
        {
            var services = new ServiceCollection();
            services.AddNodeServices();
            var serviceProvider = services
                      .AddLogging(configure => configure.AddConsole())
                      .BuildServiceProvider();
            _logFactory = serviceProvider.GetService<ILoggerFactory>();
            _nodeServices = serviceProvider.GetService<INodeServices>();
        }
        public string OutPutDirectory => AppDomain.CurrentDomain.BaseDirectory;
    }
}
