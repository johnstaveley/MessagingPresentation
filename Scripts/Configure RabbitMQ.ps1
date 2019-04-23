$ipAddressOfRabbitMQ = "192.168.99.100:15672"
$userName = "user"
$password = "LeedsSharp4"

$rabbitMQToolsModule = Get-Module | Where-Object { $_.Name -eq "RabbitMQTools" }
if ($rabbitMQToolsModule -eq $null) {
    Install-Module -Name RabbitMQTools -RequiredVersion 1.1 -Force
    }
$exchangeName = "Products"
$queueName = "LargeProducts"
$queueRoutingKey = "Large"
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]'Ssl3,Tls,Tls11,Tls12'

# Create exchange if not already present
$securePassword = ConvertTo-SecureString $password -AsPlainText -Force
$credentials = New-Object System.Management.Automation.PSCredential ($userName, $securePassword)
$exchange = Get-RabbitMQExchange -HostName $ipAddressOfRabbitMQ -Credentials $credentials -Name $exchangeName -ErrorAction Stop
if ($exchange -eq $null) {
    Add-RabbitMQExchange -Name $exchangeName -Type "topic" -HostName $ipAddressOfRabbitMQ -Credentials $credentials -VirtualHost vhost
}

# Create Queue if not already present
$queue = Get-RabbitMQQueue -HostName $ipAddressOfRabbitMQ -Credentials $credentials `
    | Where-Object { $_.Name -eq $queueName }
if ($queue -eq $null) {
    Add-RabbitMQQueue -Name $queueName -HostName $ipAddressOfRabbitMQ -Credentials $credentials  -VirtualHost vhost
}

# Create queue binding if not already present
$productQueueBinding = Get-RabbitMQQueueBinding -Name $queueName -HostName $ipAddressOfRabbitMQ -Credentials $credentials `
    | Where-Object { $_.destination -eq $exchangeName -and $_.routing_key -eq $queueRoutingKey}
if ($productQueueBinding -eq $null) {
    Add-RabbitMQQueueBinding -Name $queueName -ExchangeName $exchangeName -RoutingKey "$queueRoutingKey" -HostName $ipAddressOfRabbitMQ -Credentials $credentials -VirtualHost vhost 
}    

# Look at the aftermath of our work
Get-RabbitMQOverview -HostName $ipAddressOfRabbitMQ -Credentials $credentials

#https://github.com/mariuszwojcik/RabbitMQTools
#Get-Help Add-RabbitMQExchange
#Get-Help Add-RabbitMQQueue
#Get-Help Get-RabbitMQQueueBinding -examples
#Get-Help Add-RabbitMQQueueBinding