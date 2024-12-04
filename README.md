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



