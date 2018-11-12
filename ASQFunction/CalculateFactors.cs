using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASQFunction
{
	public static class CalculateFactors
	{
		// TODO: Connection string for output queue
		// TODO: Remove connection string from source code, relate to key vault or change key
		[FunctionName("CalculateFactors")]
		public static async Task Run([QueueTrigger("numberToBeFactored", Connection = "DefaultEndpointsProtocol=https;AccountName=messagepresentation;AccountKey=<Change Me>")] string numberToBeFactored, TraceWriter log, ICollector<string> outQueueItem)
		{
			log.Info($"C# Queue trigger function processed: {numberToBeFactored}");
			var number = Convert.ToInt64(numberToBeFactored);
			var factors = Factor(number);
			var returnString = "Hello " + number + " you have " + factors.Count + " factors. The factors are: ";
			factors.OrderBy(b => b).ToList().ForEach(a => returnString += a.ToString() + ", ");
			returnString = returnString.Trim(' ').Trim(',');
			log.Info(returnString);
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
