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
        [BlobOutput("shipment-output/{rand-guid}.txt", Connection = "AzureWebJobsStorage")]
        public string Run([QueueTrigger("shipment-queue", Connection = "AzureWebJobsStorage")] string queueMessage)
        {
            _logger.LogInformation("Processing queue message: {message}", queueMessage);

            return $"Processed Shipment: {queueMessage}";
        }
    }
}
