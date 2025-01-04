param location string = resourceGroup().location
param name string

resource staticWebApp 'Microsoft.Web/staticSites@2024-04-01' = {
  location: location
  name: name
  sku: {
    tier: 'Standard'
    name: 'Standard'
  }
  properties: {}
}

output id string = staticWebApp.id
output url string = 'https://${staticWebApp.properties.defaultHostname}'
