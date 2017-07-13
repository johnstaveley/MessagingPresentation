using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace RabbitReceiver
{
	class Program
	{

		static void Main(string[] args)
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
			String consumerTag = channel.BasicConsume(queueName, false, consumer);
			Console.ReadKey();
			channel.Close(200, "Goodbye");
			conn.Close();
		}
	}
}