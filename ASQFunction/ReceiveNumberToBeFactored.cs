using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace ASQFunction
{
    public static class ReceiveNumberToBeFactored
    {

        [FunctionName("ReceiveNumberToBeFactored")]
        public static void Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log, 
            [Queue("numberToBeFactored", Connection = "AzureWebJobsStorage")] ICollector<string> outQueueItem)
        {
			log.LogInformation("C# HTTP trigger function processed a request to factor a number");

	        // parse query parameter
	        long? number = Convert.ToInt64(req.Query["number"]);

	        // Get request body
	        string requestBody = new StreamReader(req.Body).ReadToEnd();
	        dynamic data = JsonConvert.DeserializeObject(requestBody);

			// Set name to query string or body data
			number = (number == 0 ? data?.number : null);

	        log.LogInformation("Received " + number);
	        outQueueItem.Add(number.ToString());
		}
    }
}
