@description('Location for all resources')
param location string

@description('Tags to apply to all resources')
param tags object

@description('Unique token for resource naming')
param resourceToken string

@description('Name of the AI model to deploy')
param aiModelName string

@description('SKU name for the AI model deployment')
param aiModelSkuName string

@description('Capacity for the AI model deployment')
param aiModelCapacity int

@description('Principal ID to assign Cognitive Services roles to')
param principalId string

var abbrs = loadJsonContent('../../abbreviations.json')

resource aiHub 'Microsoft.MachineLearningServices/workspaces@2024-10-01' = {
  name: '${abbrs.aiHub}${resourceToken}'
  location: location
  tags: tags
  kind: 'Hub'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    friendlyName: 'Picnic Planner AI Hub'
    description: 'AI Hub for Picnic Planner multi-agent system'
    publicNetworkAccess: 'Enabled'
  }
}

resource aiProject 'Microsoft.MachineLearningServices/workspaces@2024-10-01' = {
  name: '${abbrs.aiProject}${resourceToken}'
  location: location
  tags: tags
  kind: 'Project'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    friendlyName: 'Picnic Planner'
    description: 'AI Project for Picnic Planner agents'
    hubResourceId: aiHub.id
  }
}

resource aiServicesAccount 'Microsoft.CognitiveServices/accounts@2024-10-01' = {
  name: 'aoai-${resourceToken}'
  location: location
  tags: tags
  kind: 'OpenAI'
  sku: {
    name: 'S0'
  }
  properties: {
    customSubDomainName: 'aoai-${resourceToken}'
    publicNetworkAccess: 'Enabled'
  }
}

resource modelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2024-10-01' = {
  parent: aiServicesAccount
  name: aiModelName
  sku: {
    name: aiModelSkuName
    capacity: aiModelCapacity
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: aiModelName
      version: '2024-11-20'
    }
  }
}

// Connection from AI Hub to AI Services
resource aiHubConnection 'Microsoft.MachineLearningServices/workspaces/connections@2024-10-01' = {
  parent: aiHub
  name: 'aoai-connection'
  properties: {
    category: 'AzureOpenAI'
    authType: 'AAD'
    target: aiServicesAccount.properties.endpoint
    metadata: {
      ApiType: 'Azure'
      ResourceId: aiServicesAccount.id
    }
  }
}

// Role assignment: Cognitive Services OpenAI User for the deploying principal
resource cognitiveServicesUser 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: aiServicesAccount
  name: guid(aiServicesAccount.id, principalId, 'Cognitive Services OpenAI User')
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd' // Cognitive Services OpenAI User
    )
    principalId: principalId
    principalType: 'ServicePrincipal'
  }
}

output projectEndpoint string = aiProject.properties.discoveryUrl
output modelDeploymentName string = modelDeployment.name
output aiServicesEndpoint string = aiServicesAccount.properties.endpoint
