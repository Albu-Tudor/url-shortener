param name string
param location string = resourceGroup().location
param keyVaultName string

resource redis 'Microsoft.Cache/redis@2023-08-01' = {
  name: name
  location: location
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
    redisVersion: '6.0'
    publicNetworkAccess: 'Enabled'
    redisConfiguration:{
      'aad-enabled': 'True'
    }
  }
}

resource keyVaule 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: keyVaultName
}

resource redisCacheConnectionString 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVaule
  name: 'Redis--ConnectionString'
  properties: {
    value: '${redis.name}.redis.cache.windows.net:6380,password=${redis.listKeys().primaryKey},ssl=True,abortConnect=False'
  }
}

output id string = redis.id

