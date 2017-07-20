using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace RabbitReceiver
{
	class Program
	{

		static void Main()
		{
			var queueName = "LargeProducts";
			Console.Title = "RabbitMQ Large Product Receiver";
			ConnectionFactory factory = new ConnectionFactory
			{
				Uri = "amqp://user:LeedsSharp4@192.168.99.100:5672/vhost"
			};

			IConnection conn = factory.CreateConnection();
			IModel channel = conn.CreateModel();
			var consumer = new EventingBasicConsumer(channel);
			consumer.Received += (ch, ea) =>
			{
				var body = System.Text.Encoding.UTF8.GetString(ea.Body);
				Console.WriteLine($"Received message with body {body}");
				channel.BasicAck(ea.DeliveryTag, false);
			};
			channel.BasicConsume(queueName, false, consumer);
			int counter = 0;
			while (counter < 1000)
			{
				Thread.Sleep(5000);
				counter++;
			}
			channel.Close(200, "Goodbye");
			conn.Close();
		}
	}
}