using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Debug));
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var lf = serviceProvider.GetService<ILoggerFactory>();
            var logger = lf.CreateLogger<Example>();

            //var example = new Example(logger);

            //example.Example1_SimpleAct();
            //example.Example2_SimpleDebug();
            //example.Example3_SimpleError();
            //example.Example4_ErrorWithException();
            //example.Example5_WithConditions();
            //example.Example6_WithMarkers();
            //example.Example7_WithExceptionParameters();

            Console.ReadKey(true);
        }
    }
}
