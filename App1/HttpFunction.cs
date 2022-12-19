using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace FunctionApp1
{
    public class HttpFunction
    {
        [Function("HttpFunction")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req)
        {

        }
    }
}
