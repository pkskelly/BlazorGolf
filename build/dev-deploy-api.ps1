#!/usr/local/bin/pwsh -File
Param(
  [parameter(Mandatory=$true)][string]$ConfigurationFile,
  [switch]$SkipEnvironmentChecks        
)

#read in the configuration file
if ( -not (Test-Path -Path $ConfigurationFile -PathType Leaf)){
  Write-Host "Configuration file required - $ConfigurationFile not found!" -ForegroundColor Red
  exit
}
$config = Get-Content -Path $ConfigurationFile | ConvertFrom-Json

Write-Output $config
$CONFIGURATION=$config.configuration
$RESOURCE_GROUP_NAME=$($config.groupName) 
$API_APP_NAME=$($config.apiWebApp.apiAppName)
$API_CODE_ROOT=$($config.apiWebApp.apiCodeRoot)
$API_ZIP_FILE_NAME=$config.apiWebApp.apiAppZip

# Set internal variables
$DIST_FOLDER="../dist"
$PUBLISH_FOLDER = "./bin/$Configuration/net6.0/publish"
$SCRIPT_PATH = Split-Path $script:MyInvocation.MyCommand.Path

# Display the current user's credentials for az CLI and active Azure subscription name. 
$currentADUser = $(az ad signed-in-user show --query '{displayName:displayName, mail:mail}' -o json) | ConvertFrom-Json
$currentAzSubscription = $(az account list --output json --query '"[?isDefault == `true`].{SubscriptionName:name}"' --all) | ConvertFrom-Json
Write-Host "************************************" -ForegroundColor Yellow
Write-Host "*" -ForegroundColor Yellow
Write-Host "*    Current Azure AD User: $($currentADUser.displayName) - $($currentADUser.mail)" -ForegroundColor Yellow
Write-Host "*" -ForegroundColor Yellow
Write-Host "*    Active Azure Subscription: $($currentAzSubscription.SubscriptionName)" -ForegroundColor Yellow
Write-Host "*" -ForegroundColor Yellow
Write-Host "*    This script MAY OVERWRITE resource configurations in Azure." -ForegroundColor Yellow
if (-not $SkipEnvironmentChecks) {
    Write-Host "*" -ForegroundColor Yellow
    Write-Host "************************************" -ForegroundColor Yellow
    Write-Host "*" -ForegroundColor Yellow
    $response = read-host "Press a to abort, any other key to continue." 
    $aborted = $response -eq "a"
    if ($aborted) {
        Write-Host "Aborted by user"
        exit
    }
}
else {
    Write-Host "*" -ForegroundColor Yellow
    Write-Host "************************************"
    Sleep 2 # Pause for review of the above message and to allow the user to abort if needed.
}

function CleanDistFolder {
    # Remove the dist folder as needed
    if (Test-Path -Path $DIST_FOLDER -PathType Container) {
        Write-Verbose "Removing  $DIST_FOLDER folder..."
        Remove-Item -Path $DIST_FOLDER -Recurse -Force -Confirm:$false -ErrorAction SilentlyContinue
    }else {
        Write-Verbose " No dist folder exists..."
    }
}
    
# Clean the Dist folder 
CleanDistFolder

#  Create dist folder
Write-Verbose " Creating $DIST_FOLDER folder..."
New-Item -ItemType Directory -Path $DIST_FOLDER | Out-Null

#  Test for the resource group
$groupExists = $(az group list --query '[].name' | Where-Object {$_.contains($RESOURCE_GROUP_NAME)})
if (-not $groupExists){
    Write-Host "Resource Group $RESOURCE_GROUP_NAME does not exist - cannot deploy apps!"
} else {
    Write-Host "Deploying Apps to $RESOURCE_GROUP_NAME!"

    Write-Verbose "Current folder : $PWD" 
    Write-Verbose "Changing directory to $($API_CODE_ROOT) to begin zipping." 

    Push-Location $API_CODE_ROOT
    Write-Verbose "Current folder : $PWD" 

    Write-Host "Building api app ..." 
    dotnet publish -c "$CONFIGURATION" -o "$PUBLISH_FOLDER" 

    #  CD into the publish folder and only zip the contents of the publish folder
    Write-Verbose "Changing directory to $PUBLISH_FOLDER to zip build output." 
    Push-Location $PUBLISH_FOLDER    
    Write-Verbose "Current folder : $PWD" 

    Write-Host "Zipping $PWD to $SCRIPT_PATH/$API_ZIP_FILE_NAME"  
    Write-Verbose "Current folder : $PWD" 
    Get-ChildItem -Path $PWD -Recurse | Compress-Archive -DestinationPath "$SCRIPT_PATH/$API_ZIP_FILE_NAME"    
    # Deploy the mvc app 
    az webapp deployment source config-zip -g "$RESOURCE_GROUP_NAME" -n "$API_APP_NAME" --src "$SCRIPT_PATH/$API_ZIP_FILE_NAME" 

    Pop-Location # return to API_CODE_ROOT
    Write-Verbose "Current folder : $PWD" 
    Pop-Location # return to original folder
    Write-Verbose "Current folder : $PWD" 

}
