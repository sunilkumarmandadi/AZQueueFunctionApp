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
        public string Run([QueueTrigger("shipment-queue", Connection = "AzureWebJobsStorage")] string queueMessage, FunctionContext context)
        {
            var invocationId = context?.InvocationId.ToString() ?? "unknown";
            _logger.LogInformation("QueueToBlobFunction triggered. InvocationId: {InvocationId}", invocationId);
            _logger.LogInformation("Received queue message: {Message}", queueMessage);

            try
            {
                _logger.LogDebug("Beginning processing of message. InvocationId: {InvocationId}", invocationId);

                // ... perform processing (keep minimal to avoid changing behavior) ...

                var result = $"Processed Shipment: {queueMessage}";

                _logger.LogInformation("Successfully processed message. InvocationId: {InvocationId}", invocationId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing queue message. InvocationId: {InvocationId}", invocationId);
                throw;
            }
        }
    }
}
