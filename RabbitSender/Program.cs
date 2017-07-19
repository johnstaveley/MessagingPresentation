using RabbitMQ.Client;
using System;
using System.Threading;

namespace RabbitSender
{
	class Program
	{
		private static readonly string[] Size = {
			"Small", "Medium", "Large"
		};


		static void Main()
		{
			var exchangeName = "Products";
			Console.Title = "RabbitMQ Sender";
#if DEBUG
			Console.WriteLine("Press space to start sending messages");
			Console.ReadKey();
#endif
			ConnectionFactory factory = new ConnectionFactory
			{
				Uri = "amqp://user:LeedsSharp4@192.168.99.100:5672/vhost"
			};

			IConnection conn = factory.CreateConnection();
			IModel channel = conn.CreateModel();
			var counter = 0;
			do
			{
				var size = Size[new Random().Next(3)];
				var body = $"{DateTime.Now} - {size} Product";
				channel.BasicPublish(exchangeName, size, null, System.Text.Encoding.UTF8.GetBytes(body));
				Console.WriteLine($"Published {body}");
				counter++;
				Thread.Sleep(1000);
			} while (counter < 100);
#if DEBUG
			Console.ReadKey();
#endif
			channel.Close(200, "Goodbye");
			conn.Close();
			
		}
	}

}
