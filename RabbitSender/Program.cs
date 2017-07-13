using System;
using RabbitMQ.Client;

namespace RabbitSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
	        ConnectionFactory factory = new ConnectionFactory();
	        // "guest"/"guest" by default, limited to localhost connections
	        factory.UserName = "user";
	        factory.Password = "LeedsSharp4";
	        factory.VirtualHost = "vhost";
	        factory.HostName = "192.168.99.100";

	        IConnection conn = factory.CreateConnection();
		}
    }
}