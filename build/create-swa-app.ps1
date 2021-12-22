#!/usr/local/bin/pwsh -File

[CmdletBinding()]
param(
    [parameter(Mandatory = $true)][string]$ConfigurationFile
)

#read in the dev locla settings file
if ( -not (Test-Path -Path $ConfigurationFile -PathType Leaf)){
  Write-Host "Cannot find the specified configruation file at $ConfigurationFile!" -ForegroundColor Red
  exit
}
$configuration = Get-Content -Path $ConfigurationFile | ConvertFrom-Json

Write-Output $configuration

az account list --output table 

sleep 3

az group create -n $($configuration.groupName) -l $($configuration.location)

az staticwebapp create --name $($configuration.swaAppName) --resource-group $($configuration.groupName) `
                       --source $($configuration.repository) `
                       --location $($configuration.location) `
                       --branch $($configuration.branch) `
                       --app-location $($configuration.appLocation) `
                       --output-location $($configuration.outputLocation) `
                       --login-with-github 
                       