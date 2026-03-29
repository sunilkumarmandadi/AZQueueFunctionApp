using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AZQueueFunctionApp
{
    public class QueueToBlobFunction
    {
        private readonly ILogger<QueueToBlobFunction> _logger;

        public QueueToBlobFunction(ILogger<QueueToBlobFunction> logger)
        {
            _logger = logger;
        }

        [Function("QueueToBlobFunction")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
