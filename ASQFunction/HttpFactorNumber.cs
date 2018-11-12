using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace ASQFunction
{
	public static class HttpFactorNumber
	{

	    [FunctionName("HttpFactorNumber")]
	    public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
	    {
		    log.Info("C# HTTP trigger function processed a request to factor a number");

		    // parse query parameter
		    long? number = Convert.ToInt64(req.Query["number"]);

			// Get request body
		    string requestBody = new StreamReader(req.Body).ReadToEnd();
		    dynamic data = JsonConvert.DeserializeObject(requestBody);

			// Set name to query string or body data
			number = number == 0 ? data?.number : null;
		    if (number == null) return new BadRequestObjectResult("Please pass a name on the query string or in the request body");

			var factors = Factor(number.Value);

		    var returnString = "Hello " + number.Value + " you have " + factors.Count + " factors. The factors are: ";
		    factors.OrderBy(b => b).ToList().ForEach(a => returnString += a.ToString() + ", ");
		    Thread.Sleep(3000);
		    return new OkObjectResult(returnString.Trim(' ').Trim(','));
	    }

	    public static List<long> Factor(long number)
	    {
		    List<long> factors = new List<long>();
		    long max = (long)Math.Sqrt(number);  //round down
		    for (long factor = 1; factor <= max; ++factor)
		    { //test from 1 to the square root, or the int below it, inclusive.
			    if (number % factor == 0)
			    {
				    factors.Add(factor);
				    if (factor != number / factor)
				    { // Don't add the square root twice!  Thanks Jon
					    factors.Add(number / factor);
				    }
			    }
		    }
		    return factors;
	    }

	}
}
