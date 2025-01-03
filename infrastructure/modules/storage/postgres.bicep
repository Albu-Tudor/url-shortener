param name string
param location string = resourceGroup().location
param administratorLogin string
@secure()
param administratorLoginPassword string
param keyVaultName string

resource postgresqlServer 'Microsoft.DBforPostgreSQL/flexibleServers@2023-03-01-preview' = {
  name: name
  location: location
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    version: '15'
    storage: {
      storageSizeGB: 32
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginPassword
  }
  resource database 'databases' = {
    name: 'ranges'
  }
  resource firewallAzure 'firewallRules' = {
    name: 'allow-all-azure-internal-IPs'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: keyVaultName
}

resource postregresDbConnectionString 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  name: 'Postgres--ConnectionString'
  properties: {
    value: 'Server=${postgresqlServer.name}.postgres.database.azure.com;Database=ranges;Port=5432;User Id=${administratorLogin};Password=${administratorLoginPassword};Ssl Mode=Require;' // IMPORTANT: Use an applicaiton user for production
  }
}

output serverId string = postgresqlServer.id
