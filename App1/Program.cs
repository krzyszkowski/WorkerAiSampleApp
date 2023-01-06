// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;

namespace FunctionApp1
{
    public class Program
    {

        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    var tempConfig = builder.Build();
                    builder.AddAzureAppConfiguration(options =>
                    options.Connect(new Uri("https://SOMETHING.azconfig.io"), new DefaultAzureCredential()));
                })
                .ConfigureServices(services =>
                {
                    services.AddAzureAppConfiguration();
                    services.AddApplicationInsightsTelemetryWorkerService();
                    var sp = services.BuildServiceProvider();
                    
                    var telemetryConfiguration = sp.GetService<TelemetryConfiguration>();
                    var serverTelemetryChannel = new ServerTelemetryChannel
                    {
                        EndpointAddress = telemetryConfiguration.TelemetryChannel.EndpointAddress
                    };
                    serverTelemetryChannel.Initialize(telemetryConfiguration);
                    telemetryConfiguration.TelemetryChannel = serverTelemetryChannel;

                    var loggerConfig = new LoggerConfiguration()
                                                                .MinimumLevel.Information()
                                                                .MinimumLevel.Override("Microsoft", LogEventLevel.Error);

                    loggerConfig.WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
                })
                .ConfigureFunctionsWorkerDefaults(app =>
                {
                    app.UseAzureAppConfiguration();
                    app.AddApplicationInsights();
                    app.AddApplicationInsightsLogger();
                })
                .Build();

            host.Run();
        }
    }
}
