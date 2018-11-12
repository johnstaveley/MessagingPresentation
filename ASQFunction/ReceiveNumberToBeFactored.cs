
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ASQFunction
{
	public static class ReceiveNumberToBeFactored
    {

		// TODO: Connection string for output queue
        [FunctionName("ReceiveNumberToBeFactored")]
        public static async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, TraceWriter log, ICollector<string> outQueueItem)
        {
	        log.Info("C# HTTP trigger function processed a request.");

			log.Info("C# HTTP trigger function processed a request to factor a number");

	        // parse query parameter
	        long? number = Convert.ToInt64(req.Query["number"]);

	        // Get request body
	        string requestBody = new StreamReader(req.Body).ReadToEnd();
	        dynamic data = JsonConvert.DeserializeObject(requestBody);

			// Set name to query string or body data
			number = (number == 0 ? data?.number : null);

	        log.Info("Received " + number);
	        outQueueItem.Add(number.ToString());
		}
    }
}
