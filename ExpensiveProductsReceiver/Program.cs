namespace AllProductsReceiver
{
	class Program
	{
		static void Main(string[] args)
		{
			ASBReceiver.Receive.AzureServiceBusMessages();
		}
	}
}
