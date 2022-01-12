#!/usr/local/bin/pwsh -File

[CmdletBinding()]
param(
    [parameter(Mandatory = $true)][string]$ConfigurationFile = "config.development.json",
    [switch]$SkipEnvironmentChecks    
)

# Display the current user's credentials for az CLI and active Azure subscription name. 
$currentADUser = $(az ad signed-in-user show --query '{displayName:displayName, mail:mail}' -o json) | ConvertFrom-Json
$currentAzSubscription = $(az account list --output json --query '"[?isDefault == `true`].{SubscriptionName:name}"' --all) | ConvertFrom-Json
Write-Host "************************************"
Write-Host "*"
Write-Host "*    Current Azure AD User: $($currentADUser.displayName) - $($currentADUser.mail)"
Write-Host "*"
Write-Host "*    Active Azure Subscription: $($currentAzSubscription.SubscriptionName)"
Write-Host "*"
Write-Host "*    This script MAY OVERWRITE resource configurations in Azure." -ForegroundColor Yellow
if (-not $SkipEnvironmentChecks) {
    Write-Host "*"
    Write-Host "************************************"
    Write-Host "*"
    $response = read-host "Press a to abort, any other key to continue."
    $aborted = $response -eq "a"
    if ($aborted) {
        Write-Host "Aborted by user"
        exit
    }
}
else {
    Write-Host "*"
    Write-Host "************************************"
    Sleep 4 # Pause for review of the above message and to allow the user to abort if needed.
}

#read in the dev local settings file
if ( -not (Test-Path -Path $ConfigurationFile -PathType Leaf)){
    Write-Host "Cannot find the specified configruation file at $ConfigurationFile!" -ForegroundColor Red
    exit
}
$configuration = Get-Content -Path $ConfigurationFile | ConvertFrom-Json
  
Write-Output $configuration
$WEBAPP_SECRET_NAME="$($configuration.apiWebApp.apiAppSecretName)"
$SETTINGS_FILE="@appsettings.development.json"
$CONN_STRING_KEY="DefaultConnection"

# Create a resource group.
az group create -n $($configuration.groupName) -l $($configuration.location) | Out-Null
Write-Host "Completed creating resource group..."

# Create an App Service plan in `FREE` tier.
az appservice plan create --name $($configuration.apiWebApp.apiAppPlan) --resource-group $($configuration.groupName) --sku $($configuration.apiWebApp.apiAppPlanSku) | Out-Null
Write-Host "Completed creating App Service Plan..."

# Create a web app.
az webapp create --name $($configuration.apiWebApp.apiAppName)  --resource-group $($configuration.groupName)  --plan $($configuration.apiWebApp.apiAppPlan) | Out-Null
Write-Host "Completed creating Web App for api project..."

# Add the settings to the web app 
 az webapp config appsettings set -g $($configuration.groupName)  -n $($configuration.apiWebApp.apiAppName) --settings $SETTINGS_FILE  | Out-Null
 Write-Host "Completed app configuration settings.." 

# Get the connection string from the cosmosdb resource group 
$CONN_STRING = $(az cosmosdb keys list --name $($configuration.cosmosDb.dbAccount)  --resource-group $($configuration.cosmosDb.groupName) --type connection-strings --output json --query '"connectionStrings[?description == `Primary SQL Connection String`].{DefaultConnection:connectionString}"') | ConvertFrom-Json 
# Add customer connection setting to the app 
az webapp config connection-string set -g $($configuration.groupName)  -n $($configuration.apiWebApp.apiAppName)  -t Custom --settings "$CONN_STRING_KEY='$($CONN_STRING.DefaultConnection)'" | Out-Null
Write-Host "Completed connection string settings..." 

# Display the connection string to add to GitHub Actions secrets
az webapp deployment list-publishing-profiles --name "$($configuration.apiWebApp.apiAppName)" --resource-group $($configuration.groupName) --xml | Out-File app_pub_profile.xml
Write-Host "WEBAPP_SECRET_NAME - $($WEBAPP_SECRET_NAME)"
Get-Item -Path app_pub_profile.xml |  gh secret set $WEBAPP_SECRET_NAME
Remove-Item -Path app_pub_profile.xml
Write-Host "Completed adding publishing profile to github secrets .." 

az staticwebapp create --name $($configuration.staticWebApp.swaAppName) --resource-group $($configuration.groupName) `
                       --source $($configuration.repository) `
                       --location $($configuration.location) `
                       --branch $($configuration.branch) `
                       --app-location $($configuration.staticWebApp.wasmAppLocation) `
                       --output-location $($configuration.staticWebApp.wasmOutputLocation) `
                       --login-with-github 

Write-Host "Completed create and configuring static web app ..." 