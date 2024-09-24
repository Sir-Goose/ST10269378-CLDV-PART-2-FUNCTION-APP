// azure function for processing queue messages
using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace st10269378.functions
{
    // static class for handling queue messages
    public static class ProcessQueueMessage
    {
        // azure function for processing queue messages
        [Function("ProcessQueueMessage")]
        public static async Task<IActionResult> Run(
            // HTTP trigger for the azure function
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            // logger instance for logging information
            ILogger log)
        {
            // retrieves the queue name and message from the HTTP request query parameters
            string queueName = req.Query["queueName"];
            string message = req.Query["message"];

            // checks if the queue name and message are provided
            if (string.IsNullOrEmpty(queueName) || string.IsNullOrEmpty(message))
            {
                // returns a bad request result if the queue name or message is missing
                return new BadRequestObjectResult("Queue name and message must be provided.");
            }

            // retrieves the Azure Storage connection string from the environment variable
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");

            // creates a new QueueServiceClient instance using the connection string
            var queueServiceClient = new QueueServiceClient(connectionString);

            // retrieves the QueueClient instance for the specified queue
            var queueClient = queueServiceClient.GetQueueClient(queueName);

            // creates the queue if it does not exist
            await queueClient.CreateIfNotExistsAsync();

            // adds the message to the queue asynchronously
            await queueClient.SendMessageAsync(message);

            // returns an OK result with a success message
            return new OkObjectResult("Message added to queue");
        }
    }
}