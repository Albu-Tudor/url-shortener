# url-shortener
Build a URL Shortener.

## Infrastructure as Code

### Log in into Azure
```bash
az login
```

### Create Resource Group
```bash
az group create --name urlshortener-dev --location westeurope
```

### What if
```bash 
az deployment group what-if --resource-group urlshortener-dev --template-file .\infrastructure\main.bicep
 ```

### Deploy
```bash
az deployment group create --resource-group urlshortener-dev --template-file .\infrastructure\main.bicep
```

### Create User for GitHub Actions
```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" --role contributor --scopes /subscriptions/<subscription-id> --sdk-auth
```

### Apply to Custom Contributor Role

```bash
az ad sp create-for-rbac --name "GitHub-Actions-SP" --role 'infra_deploy' --scopes /subscriptions/<subscription-id> --sdk-auth
```

#### Configure a federal identity credential on an app

https://learn.microsoft.com/en-gb/entra/workload-id/workload-identity-federation-create-trust?pivots=identity-wif-apps-methods-azp#configure-a-federated-identity-credential-on-an-app


### Get publish profile for App Service
```bash
az webapp deployment list-publishing-profiles --name <APP_NAME> --resource-group <RESOURCE_GROUP_NAME> --xml
```