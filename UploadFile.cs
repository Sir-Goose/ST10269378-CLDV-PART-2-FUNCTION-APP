// azure function for uploading files to azure file share
using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace st10269378.functions
{
    // static class for handling file uploads
    public static class UploadFile
    {
        // azure function for uploading files
        [Function("UploadFile")]
        public static async Task<IActionResult> Run(
            // HTTP trigger for the azure function
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            // logger instance for logging information
            ILogger log)
        {
            // retrieves the share name and file name from the HTTP request query parameters
            string shareName = req.Query["shareName"];
            string fileName = req.Query["fileName"];

            // checks if the share name and file name are provided
            if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(fileName))
            {
                // returns a bad request result if either the share name or file name is missing
                return new BadRequestObjectResult("Share name and file name must be provided.");
            }

            // retrieves the Azure Storage connection string from the environment variable
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");

            // creates a new ShareServiceClient instance using the connection string
            var shareServiceClient = new ShareServiceClient(connectionString);

            // retrieves the ShareClient instance for the specified share
            var shareClient = shareServiceClient.GetShareClient(shareName);

            // creates the share if it does not exist
            await shareClient.CreateIfNotExistsAsync();

            // retrieves the root directory client for the share
            var directoryClient = shareClient.GetRootDirectoryClient();

            // retrieves the FileClient instance for the specified file
            var fileClient = directoryClient.GetFileClient(fileName);

            // uploads the file asynchronously using the HTTP request body
            using var stream = req.Body;
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadAsync(stream);

            // returns an OK result with a success message
            return new OkObjectResult("File uploaded to Azure Files");
        }
    }
}