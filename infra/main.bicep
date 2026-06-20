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

var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var tags = {
  'azd-env-name': environmentName
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
output AZURE_CONTAINER_APPS_ENVIRONMENT_ID string = containerApps.outputs.environmentId
