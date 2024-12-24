param location string = resourceGroup().location
param appServicePlanName string 
param appName string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  kind: 'linux'
  location: location
  name: appServicePlanName
  properties: {
    reserved: true
  }
  sku: {
    name: 'B1'
  }
}

resource webApi 'Microsoft.Web/sites@2022-09-01' = {
  name: appName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly:  true
    siteConfig: {
      linuxFxVersion:'DOTNETCORE|8.0'
    }
  }
}

output appServiceId string = webApi.id
