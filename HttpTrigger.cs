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
            var queueUri = new Uri("https://defaultresourcegroub043.queue.core.windows.net/shipment-queue");

            var client = new QueueClient(
                queueUri,
                new DefaultAzureCredential());

            await client.CreateIfNotExistsAsync();
            await client.SendMessageAsync("hello from http trigger");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Message added to queue.");
            return response;
        }
    }
}
