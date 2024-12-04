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

