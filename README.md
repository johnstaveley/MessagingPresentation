# Messaging Presentation
Supporting files and code for messaging presentation at Leeds Sharp 27/07/2017

## Demo 1: Azure Storage Queues and Azure Functions
* TODO: Azure functions and queues need to be pre-setup on Azure. However source code is in the scripts folder
* The script 'Hit Azure Apis.ps1' is used to demo a single hit on the http site and multiple hits on an http site backed by a queue.
* The project ASQFactoredNumbersReceiver can be run in the background to receive the factored numbers from the processed queue.

## Demo 2: Azure Service Bus using an Arm Template
* Setup the infrastructure by running the script in the ASBAzureSetup project, you will need to change the subscription id.
* Set the solution to run multiple projects: ASBSender, ASBAllProductsReceiver, ASBExpensiveProductsReceiver, ASBLightColourReceiver and ASBLargeProductsReceiver
* Press space to send the messages to the queues in ASBSender and watch them appear in the receivers

## Demo 3: RabbitMQ on Docker
* Install docker locally
* Configure RabbitMQ on docker (See Demo Notes.doc)
* Run both projects RabbitSender and RabbitReceiver
* Watch a subset of the messages appear in the receiver
