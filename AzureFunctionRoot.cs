// azure function root class for handling HTTP requests
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace st10269378.functions
{
    public class AzureFunctionRoot
    {
        // logger instance for logging information
        private readonly ILogger<AzureFunctionRoot> _logger;

        // initializes the azure function root class with a logger instance
        public AzureFunctionRoot(ILogger<AzureFunctionRoot> logger)
        {
            _logger = logger;
        }

        // HTTP trigger function for handling GET and POST requests
        [Function("HTTPTest")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            // logs information about the request being processed
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            // returns an OK result with a welcome message
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}