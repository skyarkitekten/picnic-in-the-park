#Requires -Version 7.0
<#
.SYNOPSIS
    Creates an Entra ID app registration with GitHub OIDC federated credentials.

.DESCRIPTION
    Sets up OIDC federation between GitHub Actions and Azure for the
    picnic-in-the-park repository. Creates the app registration, federated
    credentials, role assignments, and configures GitHub repository variables.

    This script is idempotent — safe to re-run.

.PARAMETER Repo
    GitHub repository in the format "owner/repo" (e.g., "skyarkitekten/picnic-in-the-park")

.PARAMETER SubscriptionId
    Azure subscription ID to deploy to

.PARAMETER Location
    Azure region for resource deployment (e.g., "eastus2")

.EXAMPLE
    ./setup-oidc.ps1 -Repo "skyarkitekten/picnic-in-the-park" -SubscriptionId "xxx" -Location "eastus2"
#>

param(
    [Parameter(Mandatory)]
    [string]$Repo,

    [Parameter(Mandatory)]
    [string]$SubscriptionId,

    [Parameter(Mandatory)]
    [string]$Location
)

$ErrorActionPreference = 'Stop'

$AppName = "picnic-in-the-park-deploy"
$AzureEnvName = "production"

Write-Host "==> Setting active subscription to $SubscriptionId"
az account set --subscription $SubscriptionId

$TenantId = (az account show --query tenantId -o tsv)
Write-Host "==> Tenant ID: $TenantId"

# Create or retrieve the app registration
Write-Host "==> Creating/retrieving app registration: $AppName"
$AppId = (az ad app list --display-name $AppName --query "[0].appId" -o tsv)

if ([string]::IsNullOrEmpty($AppId) -or $AppId -eq "None") {
    $AppId = (az ad app create --display-name $AppName --query appId -o tsv)
    Write-Host "    Created app registration: $AppId"
} else {
    Write-Host "    Found existing app registration: $AppId"
}

# Create or retrieve the service principal
Write-Host "==> Creating/retrieving service principal"
$SpId = (az ad sp list --filter "appId eq '$AppId'" --query "[0].id" -o tsv)

if ([string]::IsNullOrEmpty($SpId) -or $SpId -eq "None") {
    $SpId = (az ad sp create --id $AppId --query id -o tsv)
    Write-Host "    Created service principal: $SpId"
} else {
    Write-Host "    Found existing service principal: $SpId"
}

# Create federated credential for main branch
Write-Host "==> Creating federated credential for main branch"
$MainCredName = "github-main"
$ExistingCred = (az ad app federated-credential list --id $AppId --query "[?name=='$MainCredName'].name" -o tsv)

if ([string]::IsNullOrEmpty($ExistingCred)) {
    $credParams = @{
        name = $MainCredName
        issuer = "https://token.actions.githubusercontent.com"
        subject = "repo:${Repo}:ref:refs/heads/main"
        audiences = @("api://AzureADTokenExchange")
    } | ConvertTo-Json -Compress

    az ad app federated-credential create --id $AppId --parameters $credParams
    Write-Host "    Created federated credential for main branch"
} else {
    Write-Host "    Federated credential for main branch already exists"
}

# Create federated credential for pull requests
Write-Host "==> Creating federated credential for pull requests"
$PrCredName = "github-pr"
$ExistingPrCred = (az ad app federated-credential list --id $AppId --query "[?name=='$PrCredName'].name" -o tsv)

if ([string]::IsNullOrEmpty($ExistingPrCred)) {
    $prCredParams = @{
        name = $PrCredName
        issuer = "https://token.actions.githubusercontent.com"
        subject = "repo:${Repo}:pull_request"
        audiences = @("api://AzureADTokenExchange")
    } | ConvertTo-Json -Compress

    az ad app federated-credential create --id $AppId --parameters $prCredParams
    Write-Host "    Created federated credential for pull requests"
} else {
    Write-Host "    Federated credential for pull requests already exists"
}

# Assign Contributor role
Write-Host "==> Assigning Contributor role"
az role assignment create `
    --assignee $SpId `
    --role "Contributor" `
    --scope "/subscriptions/$SubscriptionId" `
    2>$null
Write-Host "    Role assignment created or already exists"

# Assign User Access Administrator role
Write-Host "==> Assigning User Access Administrator role"
az role assignment create `
    --assignee $SpId `
    --role "User Access Administrator" `
    --scope "/subscriptions/$SubscriptionId" `
    2>$null
Write-Host "    Role assignment created or already exists"

# Set GitHub repository variables
Write-Host "==> Setting GitHub repository variables"
gh variable set AZURE_CLIENT_ID --body $AppId --repo $Repo
gh variable set AZURE_TENANT_ID --body $TenantId --repo $Repo
gh variable set AZURE_SUBSCRIPTION_ID --body $SubscriptionId --repo $Repo
gh variable set AZURE_ENV_NAME --body $AzureEnvName --repo $Repo
gh variable set AZURE_LOCATION --body $Location --repo $Repo

Write-Host ""
Write-Host "============================================"
Write-Host "OIDC federation setup complete!"
Write-Host "============================================"
Write-Host ""
Write-Host "  App Registration:    $AppName"
Write-Host "  Client ID:           $AppId"
Write-Host "  Tenant ID:           $TenantId"
Write-Host "  Subscription ID:     $SubscriptionId"
Write-Host "  Location:            $Location"
Write-Host ""
Write-Host "GitHub repository variables have been set on $Repo."
Write-Host "The CD workflow can now authenticate via OIDC."
Write-Host ""
