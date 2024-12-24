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

### Prediction for resources
```bash 
 az deployment group what-if --resource-group urlshortener-dev --template-file .\infrastructure\main.bicep
 ```

# Create resources on Azure
 ```bash
 az deployment group create --resource-group urlshortener-dev --template-file .\infrastructure\main.bicep
 ```