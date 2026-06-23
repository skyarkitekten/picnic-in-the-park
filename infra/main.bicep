targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment (e.g., production, staging)')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

@description('Principal ID of the deploying user or service principal')
param principalId string

@description('Name of the AI model deployment')
param aiModelName string = 'gpt-4o'

@description('SKU for the AI model deployment')
param aiModelSkuName string = 'GlobalStandard'

@description('Capacity for the AI model deployment (in thousands of tokens per minute)')
param aiModelCapacity int = 10

@description('Owner tag required by subscription policy')
param owner string = 'picnic-in-the-park'

@description('Purpose tag required by subscription policy')
param purpose string = 'Validate issue 17 current state'

@description('Billing Code tag required by subscription policy')
param billingCode string = 'Demo'

var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var tags = {
  'azd-env-name': environmentName
  Owner: owner
  Purpose: purpose
  'Billing Code': billingCode
  BillingCode: billingCode
}

resource rg 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'rg-picnic-planner-${environmentName}'
  location: location
  tags: tags
}

module foundry 'core/ai/foundry.bicep' = {
  name: 'foundry'
  scope: rg
  params: {
    location: location
    tags: tags
    resourceToken: resourceToken
    aiModelName: aiModelName
    aiModelSkuName: aiModelSkuName
    aiModelCapacity: aiModelCapacity
    principalId: principalId
    appPrincipalId: containerApps.outputs.registryManagedIdentityPrincipalId
  }
}

module containerApps 'core/host/container-apps.bicep' = {
  name: 'container-apps'
  scope: rg
  params: {
    location: location
    tags: tags
    resourceToken: resourceToken
  }
}

module redis 'core/cache/redis.bicep' = {
  name: 'redis'
  scope: rg
  params: {
    location: location
    tags: tags
    resourceToken: resourceToken
  }
}

output AZURE_RESOURCE_GROUP string = rg.name
output AZURE_AI_PROJECT_ENDPOINT string = foundry.outputs.projectEndpoint
output AZURE_AI_MODEL_DEPLOYMENT_NAME string = foundry.outputs.modelDeploymentName
output AZURE_CONTAINER_REGISTRY_ENDPOINT string = containerApps.outputs.registryLoginServer
output AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_ID string = containerApps.outputs.registryManagedIdentityId
output AZURE_CONTAINER_REGISTRY_MANAGED_IDENTITY_CLIENT_ID string = containerApps.outputs.registryManagedIdentityClientId
output MANAGED_IDENTITY_CLIENT_ID string = containerApps.outputs.registryManagedIdentityClientId
output MANAGED_IDENTITY_NAME string = containerApps.outputs.registryManagedIdentityName
output MANAGED_IDENTITY_PRINCIPAL_ID string = containerApps.outputs.registryManagedIdentityPrincipalId
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = containerApps.outputs.environmentId
output AZURE_CONTAINER_APPS_ENVIRONMENT_NAME string = containerApps.outputs.environmentName
output AZURE_CONTAINER_APPS_ENVIRONMENT_DEFAULT_DOMAIN string = containerApps.outputs.environmentDefaultDomain
output APPLICATIONINSIGHTS_CONNECTION_STRING string = containerApps.outputs.appInsightsConnectionString
output APPLICATIONINSIGHTS_INSTRUMENTATION_KEY string = containerApps.outputs.appInsightsInstrumentationKey
output AZURE_LOG_ANALYTICS_WORKSPACE_ID string = containerApps.outputs.logAnalyticsWorkspaceId
output AZURE_LOG_ANALYTICS_WORKSPACE_NAME string = containerApps.outputs.logAnalyticsWorkspaceName
