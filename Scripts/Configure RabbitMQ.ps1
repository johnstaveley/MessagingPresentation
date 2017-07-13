$ipAddressOfRabbitMQ = "192.168.99.100"
$userName = "user"
$password = "LeedsSharp4"

# You will need to git clone them from https://github.com/mariuszwojcik/RabbitMQTools and then change to the install directory
cd "C:/Program Files (x86)/RabbitMQTools" 
$rabbitMQToolsModule = Get-Module | Where-Object { $_.Name -eq "RabbitMQTools" }
if ($rabbitMQToolsModule -eq $null) {
    Import-Module .\RabbitMQTools
    }
$exchangeName = "Products"
$queueName = "Products"
$queueRoutingKey = "Large"

# Create exchange if not already present
$exchange = Get-RabbitMQExchange -ComputerName $ipAddressOfRabbitMQ -UserName $userName -Password $password | Where-Object { $_.Name -eq $exchangeName}
if ($exchange -eq $null) {
    Add-RabbitMQExchange -Name $exchangeName -Type "topic" -ComputerName $ipAddressOfRabbitMQ -UserName $userName -Password $password -VirtualHost vhost
}

# Create Queue if not already present
$queue = Get-RabbitMQQueue -ComputerName $ipAddressOfRabbitMQ -UserName $userName -Password $password | Where-Object { $_.Name -eq $queueName }
if ($queue -eq $null) {
    Add-RabbitMQQueue -Name $queueName -ComputerName $ipAddressOfRabbitMQ -UserName $userName -Password $password  -VirtualHost vhost
}

# Create queue binding if not already present
$productQueueBinding = Get-RabbitMQQueueBinding -Name $queueName -ComputerName $ipAddressOfRabbitMQ -UserName $userName -Password $password | Where-Object { $_.destination -eq $exchangeName -and $_.routing_key -eq $queueRoutingKey}
if ($productQueueBinding -eq $null) {
    Add-RabbitMQQueueBinding -Name $queueName -ExchangeName $exchangeName -RoutingKey "$queueRoutingKey" -ComputerName $ipAddressOfRabbitMQ -UserName $userName -Password $password -VirtualHost vhost 
}    

# Look at the aftermath of our work
Get-RabbitMQOverview -ComputerName $ipAddressOfRabbitMQ -UserName $userName -Password $password

#https://github.com/mariuszwojcik/RabbitMQTools
#Get-Help Add-RabbitMQExchange
#Get-Help Add-RabbitMQQueue
#Get-Help Get-RabbitMQQueueBinding -examples
#Get-Help Add-RabbitMQQueueBinding