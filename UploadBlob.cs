// azure function for uploading blobs
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace st10269378.functions
{
    // static class for handling blob uploads
    public static class UploadBlob
    {
        // azure function for uploading blobs
        [Function("UploadBlob")]
        public static async Task<IActionResult> Run(
            // HTTP trigger for the azure function
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            // logger instance for logging information
            ILogger log)
        {
            // retrieves the container name and blob name from the HTTP request query parameters
            string containerName = req.Query["containerName"];
            string blobName = req.Query["blobName"];

            // checks if the container name and blob name are provided
            if (string.IsNullOrEmpty(containerName) || string.IsNullOrEmpty(blobName))
            {
                // returns a bad request result if either the container name or blob name is missing
                return new BadRequestObjectResult("Container name and blob name must be provided.");
            }

            // retrieves the Azure Storage connection string from the environment variable
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");

            // creates a new BlobServiceClient instance using the connection string
            var blobServiceClient = new BlobServiceClient(connectionString);

            // retrieves the BlobContainerClient instance for the specified container
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // creates the container if it does not exist
            await containerClient.CreateIfNotExistsAsync();

            // retrieves the BlobClient instance for the specified blob
            var blobClient = containerClient.GetBlobClient(blobName);

            // uploads the blob asynchronously using the HTTP request body
            using var stream = req.Body;
            await blobClient.UploadAsync(stream, true);

            // returns an OK result with a success message
            return new OkObjectResult("Blob uploaded");
        }
    }
}