param location string = resourceGroup().location

var uniqueId = uniqueString(resourceGroup().id)

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
    keyVaultName: keyVault.outputs.name
  }
  dependsOn: [
    keyVault
  ]
}

module keyVaultRoleAssigment 'modules/secrets/key-vault-role-assignment.bicep' = {
  name: 'keyVaultRoleAssigmentDeployment'
  params: {
    keyVaultName: keyVault.outputs.name
    principalIds: [
      apiService.outputs.principalId
      // Add more principal IDs as needed
    ]
  }
  dependsOn: [
    keyVault
    apiService
  ]
}

