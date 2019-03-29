using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Reporting.Test
{
    public class TestBase
    {
        protected readonly ILoggerFactory _logFactory ;
        public TestBase()
        {
            var serviceProvider = new ServiceCollection()
                      .AddLogging(configure => configure.AddConsole())
                      .BuildServiceProvider();
            _logFactory = serviceProvider.GetService<ILoggerFactory>();
        }
    }
}
