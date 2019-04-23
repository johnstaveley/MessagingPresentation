using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASQFunction
{
    public static class CalculateFactors
	{
		[FunctionName("CalculateFactors")]
		public static void Run([QueueTrigger("numberToBeFactored", Connection = "AzureWebJobsStorage")] string numberToBeFactored, ILogger log, [Queue("factorednumbers", Connection = "AzureWebJobsStorage")] ICollector<string> outQueueItem)
		{
			log.LogInformation($"C# Queue trigger function processed: {numberToBeFactored}");
			var number = Convert.ToInt64(numberToBeFactored);
			var factors = Factor(number);
			var returnString = "Hello " + number + " you have " + factors.Count + " factors. The factors are: ";
			factors.OrderBy(b => b).ToList().ForEach(a => returnString += a.ToString() + ", ");
			returnString = returnString.Trim(' ').Trim(',');
			log.LogInformation(returnString);
			outQueueItem.Add(returnString);
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
