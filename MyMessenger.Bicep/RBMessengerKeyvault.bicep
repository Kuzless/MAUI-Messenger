param location string
param webAppId string
param dbConStr string
param blobConStr string

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: 'RBMessengerKeyvault'
  location: location
  properties: {
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: '0231332b-245e-4115-b129-58b5f1109b20'
        permissions: {
          keys: [
            'all'
          ]
          secrets: [
            'all'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: webAppId
        permissions: {
          keys: [
            'all'
          ]
          secrets: [
            'all'
          ]
        }
      }
    ]
    sku: {
      name: 'standard'
      family: 'A'
    }
  }
}

resource keyVaultSecret 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: 'DatabaseConnectionString'
  parent: keyVault
  properties: {
    value: dbConStr
  }
}
resource keyVaultSecret2 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: 'JWTSecretKey'
  parent: keyVault
  properties: {
    value: 'prod_secret_key_private_123012301230'
  }
}
resource keyVaultSecret3 'Microsoft.KeyVault/vaults/secrets@2023-07-01' = {
  name: 'BlobConnectionString'
  parent: keyVault
  properties: {
    value: blobConStr
  }
}

