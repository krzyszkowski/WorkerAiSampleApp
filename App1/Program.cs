// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

namespace FunctionApp1
{
    public class Program
    {
        const string AiConnectionString = "AI_CONNECTION_STRING_HERE"; // in real it would be retrieved from ACC

        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults((workerApplication) =>
                {
                    workerApplication.AddApplicationInsights(o => o.ConnectionString = AiConnectionString);
                    workerApplication.AddApplicationInsightsLogger();
                })
                .Build();

            host.Run();
        }
    }
}
