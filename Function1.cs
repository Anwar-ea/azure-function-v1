using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace axiom_func
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string method = req.Method;
            if (req.Method.Equals("post", System.StringComparison.InvariantCultureIgnoreCase))
            {
                string body = await new StreamReader(req.Body).ReadToEndAsync();
                try
                {
                    Todo data = JsonConvert.DeserializeObject<Todo>(body);
                    return new OkObjectResult(data);
                }
                catch
                {
                    // If not valid JSON, return as plain text
                    return new OkObjectResult(body);
                }
            }

            return new OkObjectResult("Welcome to Azure Functions 123!");
        }
    }

    public class Todo
    {
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
