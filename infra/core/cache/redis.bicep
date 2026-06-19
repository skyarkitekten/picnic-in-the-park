@description('Location for all resources')
param location string

@description('Tags to apply to all resources')
param tags object

@description('Unique token for resource naming')
param resourceToken string

var abbrs = loadJsonContent('../../abbreviations.json')

resource redis 'Microsoft.Cache/redis@2024-03-01' = {
  name: '${abbrs.redisCache}${resourceToken}'
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
    enableNonSslPort: false
    minimumTlsVersion: '1.2'
  }
}

output redisHostName string = redis.properties.hostName
output redisId string = redis.id
