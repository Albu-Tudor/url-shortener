param location string = resourceGroup().location
@secure()
param pgSqlPassword string

var uniqueId = uniqueString(resourceGroup().id)
var keyVaultName = 'kv-${uniqueId}'

module keyVault 'modules/secrets/keyvault.bicep' = {
  name: 'keyVaultDeployment'
  params: {
    vaultName: 'kv-${uniqueId}'
    location: location
  }
}

module apiService 'modules/compute/appservice.bicep' = {
  name: 'apiDeployment'
  params: {
    appName: 'api-${uniqueId}'
    appServicePlanName: 'plan-api-${uniqueId}'
    location: location
    keyVaultName: keyVaultName
    appSettings: [
      {
        name: 'DatabaseName'
        value: 'urls'
      }
      {
        name: 'ContainerName'
        value: 'items'
      }
      {
        name: 'ByUserDatabaseName'
        value: 'urls'
      }
      {
        name: 'ByUserContainerName'
        value: 'byUser'
      }
      {
        name: 'TokenRangeService__Endpoint'
        value: tokenRangeService.outputs.url
      }
      {
        name: 'AzureAd__Instance'
        value: environment().authentication.loginEndpoint
      }
      {
        name: 'AzureAd__TenantId'
        value: tenant().tenantId
      }
      {
        name: 'AzureAd__ClientId'
        value: entraApp.outputs.appId
      }
      {
        name: 'AzureAd__Scopes'
        value: 'Urls.Read'
      }
      {
        name: 'WebAppEndpoints'
        value: '${staticWebApp.outputs.url};http://localhost:3000'
      }
    ]
  }
}

module tokenRangeService 'modules/compute/appservice.bicep' = {
  name: 'tokenRangeSeviceDeployment'
  params: {
    appName: 'token-range-service-${uniqueId}' 
    appServicePlanName: 'plan-token-range-${uniqueId}'
    location: location
    keyVaultName: keyVaultName
  }
}

module redirectApiService 'modules/compute/appservice.bicep' = {
  name: 'redirectApiSeviceDeployment'
  params: {
    appName: 'redirect-api-${uniqueId}' 
    appServicePlanName: 'plan-redirect-api-${uniqueId}'
    location: location
    keyVaultName: keyVaultName
    appSettings: [
      {
        name: 'DatabaseName'
        value: 'urls'
      }
      {
        name: 'ContainerName'
        value: 'items'
      }
    ]
  }
}

module storageAccount 'modules/storage/storage-account.bicep' = {
  name: 'storageAccountDeployment'
  params: {
    name: 'storage${uniqueId}'
    location: location
  }
}

module cosmosTriggerFunction 'modules/compute/function.bicep' = {
  name: 'cosmosTriggerFunctionDeployment'
  params: {
    appServicePlanName: 'plan-cosmos-trigger-${uniqueId}'
    name: 'cosmos-trigger-function-${uniqueId}' 
    location: location
    keyVaultName: keyVaultName
    storageAccountConnectionString: storageAccount.outputs.storageAccountConnectionString
    appSettings: [
      {
        name: 'CosmosDbConnection'
        value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/CosmosDb--ConnectionString/)'
      }
      {
        name: 'TargetDatabaseName'
        value: 'urls'
      }
      {
        name: 'TargetContainerName'
        value: 'byUser'
      }
    ]
  }
}

module postres 'modules/storage/postgres.bicep' = {
  name: 'postgresDeployment'
  params: {
    name: 'postres-${uniqueId}' 
    location: location
    administratorLogin: 'adminuser'
    administratorLoginPassword: pgSqlPassword
    keyVaultName: keyVaultName
  }
}

module cosmosDb 'modules/storage/cosmos-db.bicep' = {
  name: 'cosmosDbDeployment'
  params: {
    name: 'cosmos-db-${uniqueId}'
    location: location
    kind: 'GlobalDocumentDB'
    databaseName: 'urls'
    locationName: 'Spain Central'
    keyVaultName: keyVaultName
  }
}

module keyVaultRoleAssignment 'modules/secrets/key-vault-role-assignment.bicep' = {
  name: 'keyVaultRoleAssignmentDeployment'
  params: {
    keyVaultName: keyVaultName
    principalIds: [
      apiService.outputs.principalId
      tokenRangeService.outputs.principalId
      redirectApiService.outputs.principalId
      cosmosTriggerFunction.outputs.principalId
    ]
  }
}

module redisCache 'modules/storage/redis-cache.bicep' = {
  name: 'redisCacheDeployment'
  params: {
    name: 'redis-catch-${uniqueId}'
    location: location
    keyVaultName: keyVaultName
  }
}

module staticWebApp 'modules/web/static-web-app.bicep' = {
  name: 'staticWebAppDeployment'
  params: {
    name: 'web-app-${uniqueId}'
    location: location
  }
}

module entraApp 'modules/identity/entra-app.bicep' = {
  name: 'entraAppWeb'
  params: {
    applicationName: 'web-${uniqueId}'
    spaRedirectUrls: [
      'http://localhost:3000/'
      staticWebApp.outputs.url
    ]
  }
}
