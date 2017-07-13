using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;

namespace ASBReceiver
{
	public static class Receive
	{
		public static void AzureServiceBusMessages()
		{

			string TopicName = "Products";
			string subscriptionName = ConfigurationManager.AppSettings["SubscriptionName"];
			Console.Title = $"ASB Receiver of '{subscriptionName}'";

			Console.WriteLine("Subscription: {0}", subscriptionName);
			Console.WriteLine();
			SubscriptionClient productsTopicClient = SubscriptionClient.CreateFromConnectionString(ConfigurationManager.AppSettings["ServiceBusConnectionString"], TopicName, subscriptionName, ReceiveMode.PeekLock);

			productsTopicClient.OnMessageAsync(
			async message =>
			{
				try
				{
					string messageReceived = message.GetBody<string>();
					Console.WriteLine("Received Message '{0}'", messageReceived);
					await message.CompleteAsync();
				}
				catch (Exception exception)
				{
					Console.Error.WriteLine(exception);
					await message.DeadLetterAsync();
				}
			}, new OnMessageOptions { AutoComplete = false, MaxConcurrentCalls = 1 });

			Console.ReadKey();
		}
	}
}
