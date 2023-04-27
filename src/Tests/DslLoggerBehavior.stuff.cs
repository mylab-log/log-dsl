﻿using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log;
using MyLab.Log.Dsl;
using MyLab.Log.XUnit;
using Xunit.Abstractions;

namespace Tests
{
    public partial class DslLoggerBehavior
    {
        private readonly ITestOutputHelper _output;

        public DslLoggerBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        private IDslLogger CreateCoreLogger()
        {
            var services = new ServiceCollection()
                .AddLogging(c => c
                    .AddXUnit(_output)
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddMyLabConsole()
                )
                .BuildServiceProvider();

            var loggerFactory = services.GetService<ILoggerFactory>();

            return loggerFactory.CreateLogger<DslLoggerBehavior>().Dsl();
        }
    }
}