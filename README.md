# url-shortener
Build a URL shortener

## Infrastructure as Code

### Login in into Azure
```bash
az login
```

### Create Resource Group
```bash
az group create --name urlshortener-dev --location westeurope
```

### what-if command
```bash
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### deploy command
```bash
az deployment group what-if --resource-group urlshortener-dev --template-file infrastructure/main.bicep
```

### Create User for GitHub Actions
```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" \
                         --role contributor \
                         --scopes subscriptions/subscriptionId/ \
                         --sdk-auth
```

### Configure a federated identity credential on an app

https://learn.microsoft.com/en-gb/entra/workload-id/workload-identity-federation-create-trust?pivots=identity-wif-apps-methods-azp#configure-a-federated-identity-credential-on-an-app

### Get the AZURE_API_PUBLISH_PROFILE variable
```baash
az webapp deployment list-publishing-profiles --name {WEBAPP_NAME} --resource-group {RESOURCE_GROUP_NAME} --xml
```
