<#
 .SYNOPSIS
    Deploys a template to Azure

 .DESCRIPTION
    Deploys an Azure Resource Manager template

#>

$subscriptionId = "a0d704f5-f3ad-49cb-ad02-bda68d45ed1f"
$resourceGroupLocation = "uksouth"
$environmentName = "LeedsSharp2"

<#
.SYNOPSIS
    Registers RPs
#>
Function RegisterRP {
    Param(
        [string]$ResourceProviderNamespace
    )

    Write-Host "Registering resource provider '$ResourceProviderNamespace'";
    Register-AzureRmResourceProvider -ProviderNamespace $ResourceProviderNamespace;
}

function Check-Session () {
    $Error.Clear()
    #if context already exist
    Get-AzureRmContext -ErrorAction Continue
    $Error
    foreach ($eachError in $Error) {
        if ($eachError.Exception.ToString() -like "*Run Login-AzureRmAccount to login.*") {
            Login-AzureRmAccount
        }
    }
    $Error.Clear();
}

# Setup variables
$workingDirectory = "C:\Work\MessagingPresentation\ServiceBusSetup\"
$parametersFile = "azuredeploy.parameters.json"
$templateFile = "azuredeploy.json"
$templateFilePath = $workingDirectory + $templateFile
$parametersFilePath = $workingDirectory + $parametersFile
$resourceGroupName = $environmentName
$ErrorActionPreference = "Stop"

# sign in
Write-Host "Logging in...";
Check-Session

# select subscription
Write-Host "Selecting subscription '$subscriptionId'";
Select-AzureRmSubscription -SubscriptionID $subscriptionId;

# Register RPs
$resourceProviders = @("microsoft.insights","microsoft.sql","microsoft.storage","microsoft.web","microsoft.servicebus");
if($resourceProviders.length) {
    Write-Host "Registering resource providers"
    foreach($resourceProvider in $resourceProviders) {
        RegisterRP($resourceProvider);
    }
}

#Create or check for existing resource group
$resourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName -ErrorAction SilentlyContinue
if(!$resourceGroup)
{
    Write-Host "Resource group '$resourceGroupName' does not exist. To create a new resource group, please enter a location.";
    if(!$resourceGroupLocation) {
        $resourceGroupLocation = Read-Host "resourceGroupLocation";
    }
    Write-Host "Creating resource group '$resourceGroupName' in location '$resourceGroupLocation'";
    New-AzureRmResourceGroup -Name $resourceGroupName -Location $resourceGroupLocation
    Start-Sleep(5) # Give it some time to create the resource group
}
else{
}

# Start the deployment
Write-Host "Starting deployment...";
if(Test-Path $parametersFilePath) {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath -TemplateParameterFile $parametersFilePath -Verbose;
} else {
    New-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroupName -TemplateFile $templateFilePath;
}