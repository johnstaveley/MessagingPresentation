using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

namespace FactoredNumbersReceiver
{
	class Program
	{

		static void Main(string[] args)
		{
			var storageAccountKey = ConfigurationManager.AppSettings["StorageAccountKey"];
			if (storageAccountKey == "CHANGEME") { throw new Exception("You need to set the storage account key for this demo");}
			var storageCredentials = new StorageCredentials(ConfigurationManager.AppSettings["StorageAccount"],
				storageAccountKey);
			var storageAccount = new CloudStorageAccount(storageCredentials, true);
			var queueClient = storageAccount.CreateCloudQueueClient();
			var storageQueueName = ConfigurationManager.AppSettings["StorageQueueName"];
			var queue = queueClient.GetQueueReference(storageQueueName);
			Console.Title = $"Azure Storage Queue Receiver of '{storageQueueName}'";
			queue.CreateIfNotExists();
			int messageNumber = 1;
			DateTime startDate = DateTime.Now;
			do
			{
				var messages = queue.GetMessages(32, TimeSpan.FromMinutes(2));
				foreach (var message in messages)
				{
					// if processing was not possible, delete the messagecheck for unprocessable messages
					if (message.DequeueCount < 5)
					{
						var timeElapsed = DateTime.Now - startDate;
						Console.WriteLine($"#{messageNumber} @ {timeElapsed.Minutes * 60 + timeElapsed.Seconds} seconds: {message.AsString}");
					}

					// delete message so that it becomes invisible for other workers
					queue.DeleteMessageAsync(message);
					messageNumber++;
				}
				Thread.Sleep(1000);
			} while (true);

		}
	}
}
