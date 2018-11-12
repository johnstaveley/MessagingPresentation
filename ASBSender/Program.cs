using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;
using System.Threading;

namespace ASBSender
{

	class Program
	{

		private const string TopicName = "Products";
		private static readonly string[] Colours = { "Green", "Blue", "Red", "Orange", "Pink", "Yellow", "Black", "White" };
		private static readonly string[] Size = { "Small", "Medium", "Large"
		};

		static void Main()
		{
			Console.Title = $"Azure Service Bus Message Sender to topic subscription '{TopicName}'";
			Console.WriteLine();
			TopicClient productsTopicClient = TopicClient.CreateFromConnectionString(ConfigurationManager.AppSettings["ServiceBusConnectionString"], TopicName);

			bool moreMessages;
			Console.WriteLine("Press space to start sending messages");
			Console.ReadKey();
			do
			{
				Console.WriteLine("Sending 10 products");
				for (int i = 0; i < 10; i++)
				{
					var colour = Colours[new Random().Next(7)];
					var size = Size[new Random().Next(3)];
					var cost = new Random().Next(100);
					var message = new BrokeredMessage($"{size}.{colour}.{cost}");
					message.Properties["Colour"] = colour;
					message.Properties["Size"] = size;
					message.Properties["Cost"] = cost;
					WriteToQueue(productsTopicClient, message);
				}
				Console.WriteLine("Press space for another 10 messages, press any other key to quit");
				moreMessages = Console.ReadKey().KeyChar == ' ';
			} while (moreMessages);

		}
		private static void WriteToQueue(TopicClient queueClient, BrokeredMessage message)
		{
			int counter = 0;
			while (counter < 10)
			{
				try
				{
					queueClient.Send(message);
					break;
				}
				catch (MessagingException e)
				{
					if (!e.IsTransient)
					{
						throw;
					}
					Thread.Sleep(2000);
				}
				counter++;
			}
		}
	}
}
