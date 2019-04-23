
# Choose a demo and number of jobs to send
$demo = "azure storage queue demo site"
#$url = "http://localhost:7071/api/HttpFactorNumber"
#$url = "http://localhost:7071/api/ReceiveNumberToBeFactored"

$url = "https://messagepresentation.azurewebsites.net/api/HttpFactorNumber"
#$url = "https://messagepresentation.azurewebsites.net/api/ReceiveNumberToBeFactored"

$randomNumber = Get-Random -minimum 1 -maximum 9999999999

$maxConcurrentJobs = 10
# End of user defined variables 

[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]'Ssl3,Tls,Tls11,Tls12'

if ($maxConcurrentJobs -eq 1) {

    Invoke-RestMethod -Method Post -Uri $url -Body "{'number': $randomNumber}" -Headers @{"Content-Type" = "application/json; charset=utf-8"}

} else { 

    [int64[]] $randoms = New-Object System.Collections.ArrayList
    For ($i=0; $i -le $maxConcurrentJobs - 1; $i++) 
        {
        $randoms += Get-Random -minimum 1 -maximum 9999999999
        }

    # Create a runspace pool where $maxConcurrentJobs is the
    # maximum number of runspaces allowed to run concurrently
    $Runspace = [runspacefactory]::CreateRunspacePool(1, $maxConcurrentJobs)

    # Open the runspace pool (very important)
    $Runspace.Open()

    foreach ($random in $randoms) {
        # Create a new PowerShell instance and tell it to execute in our runspace pool
        $ps = [powershell]::Create()
        $ps.RunspacePool = $Runspace

        # Attach some code to it
        [void]$ps.AddCommand("Invoke-WebRequest").AddParameter("UseBasicParsing",$true).AddParameter("Uri",$url).AddParameter("Headers", @{"Content-Type" = "application/json; charset=utf-8"}).AddParameter("Body", "{'number':$random}").AddParameter("Method","Post")

        # Begin execution asynchronously (returns immediately)
        [void]$ps.BeginInvoke()

        # Give feedback on how far we are
        Write-Host ("Initiated request for {0} with number {1}" -f $demo, $random)
    }

}