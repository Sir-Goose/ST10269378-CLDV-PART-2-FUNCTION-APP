// azure function for storing table information
using Azure.Data.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace st10269378.functions
{
    // static class for handling table information
    public static class StoreTableInfo
    {
        // azure function for storing table information
        [Function("StoreTableInfo")]
        public static async Task<IActionResult> Run(
            // HTTP trigger for the azure function
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            // logger instance for logging information
            ILogger log)
        {
            // retrieves the table name, partition key, row key, and data from the HTTP request query parameters
            string tableName = req.Query["tableName"];
            string partitionKey = req.Query["partitionKey"];
            string rowKey = req.Query["rowKey"];
            string data = req.Query["data"];

            // checks if the table name, partition key, row key, and data are provided
            if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(partitionKey) || string.IsNullOrEmpty(rowKey) || string.IsNullOrEmpty(data))
            {
                // returns a bad request result if any of the required information is missing
                return new BadRequestObjectResult("Table name, partition key, row key, and data must be provided.");
            }

            // retrieves the Azure Storage connection string from the environment variable
            var connectionString = Environment.GetEnvironmentVariable("AzureStorage:ConnectionString");

            // creates a new TableServiceClient instance using the connection string
            var serviceClient = new TableServiceClient(connectionString);

            // retrieves the TableClient instance for the specified table
            var tableClient = serviceClient.GetTableClient(tableName);

            // creates the table if it does not exist
            await tableClient.CreateIfNotExistsAsync();

            // creates a new TableEntity instance with the partition key and row key
            var entity = new TableEntity(partitionKey, rowKey) { ["Data"] = data };

            // adds the entity to the table asynchronously
            await tableClient.AddEntityAsync(entity);

            // returns an OK result with a success message
            return new OkObjectResult("Data added to table");
        }
    }
}