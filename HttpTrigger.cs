using Azure.Identity;
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AZQueueFunctionApp
{
    public class HttpTrigger
    {
        private readonly ILogger<HttpTrigger> _logger;

        public HttpTrigger(ILogger<HttpTrigger> logger)
        {
            _logger = logger;
        }

        [Function("HttpTrigger")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("HttpTrigger function invoked.");

            var queueUri = new Uri("https://defaultresourcegroub043.queue.core.windows.net/shipment-queue");
            _logger.LogInformation("Queue URI: {QueueUri}", queueUri);

            try
            {
                var client = new QueueClient(
                    queueUri,
                    new DefaultAzureCredential(),
                    new QueueClientOptions
                    {
                        MessageEncoding = QueueMessageEncoding.Base64
                    });

                _logger.LogInformation("QueueClient created for queue: {QueueName}", client.Name);


                var messageText = "hello from http trigger";
                var sendResponse = await client.SendMessageAsync(messageText);
                _logger.LogInformation("Message sent to queue '{QueueName}'. Status: {Status}", client.Name, sendResponse.GetRawResponse().Status);

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync("Message added to queue.");
                _logger.LogInformation("HttpTrigger function completed successfully.");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add message to queue.");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("Failed to add message to queue.");
                return response;
            }
        }
    }
}
