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

@description('Managed identity principal ID used by deployed apps to call Foundry')
param appPrincipalId string = ''

var abbrs = loadJsonContent('../../abbreviations.json')

resource aiServicesAccount 'Microsoft.CognitiveServices/accounts@2025-06-01' = {
  name: 'ai-${resourceToken}'
  location: location
  tags: tags
  kind: 'AIServices'
  sku: {
    name: 'S0'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    allowProjectManagement: true
    customSubDomainName: 'ai-${resourceToken}'
    disableLocalAuth: true
    publicNetworkAccess: 'Enabled'
    networkAcls: {
      defaultAction: 'Allow'
      virtualNetworkRules: []
      ipRules: []
    }
  }

  resource modelDeployment 'deployments' = {
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

  resource project 'projects' = {
    name: '${abbrs.aiProject}${resourceToken}'
    location: location
    identity: {
      type: 'SystemAssigned'
    }
    properties: {
      description: 'AI Project for Picnic Planner agents'
      displayName: 'Picnic Planner'
    }
    dependsOn: [
      modelDeployment
    ]
  }
}

// Azure AI User for the deploying principal, scoped to the Foundry project.
resource azureAIUser 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: aiServicesAccount::project
  name: guid(aiServicesAccount::project.id, principalId, 'Azure AI User')
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '53ca6127-db72-4b80-b1b0-d745d6d5456d' // Azure AI User
    )
    principalId: principalId
  }
}

// Cognitive Services OpenAI User for direct model invocation permissions.
resource cognitiveServicesUser 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: aiServicesAccount
  name: guid(aiServicesAccount.id, principalId, 'Cognitive Services OpenAI User')
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd' // Cognitive Services OpenAI User
    )
    principalId: principalId
  }
}

// Azure AI User for deployed app managed identity, scoped to the Foundry project.
resource appAzureAIUser 'Microsoft.Authorization/roleAssignments@2022-04-01' = if (!empty(appPrincipalId)) {
  scope: aiServicesAccount::project
  name: guid(aiServicesAccount::project.id, appPrincipalId, 'Azure AI User')
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '53ca6127-db72-4b80-b1b0-d745d6d5456d' // Azure AI User
    )
    principalId: appPrincipalId
  }
}

// Cognitive Services OpenAI User for deployed app managed identity.
resource appCognitiveServicesUser 'Microsoft.Authorization/roleAssignments@2022-04-01' = if (!empty(appPrincipalId)) {
  scope: aiServicesAccount
  name: guid(aiServicesAccount.id, appPrincipalId, 'Cognitive Services OpenAI User')
  properties: {
    roleDefinitionId: subscriptionResourceId(
      'Microsoft.Authorization/roleDefinitions',
      '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd' // Cognitive Services OpenAI User
    )
    principalId: appPrincipalId
  }
}

output projectEndpoint string = aiServicesAccount::project.properties.endpoints['AI Foundry API']
output modelDeploymentName string = aiServicesAccount::modelDeployment.name
output aiServicesEndpoint string = aiServicesAccount.properties.endpoint
