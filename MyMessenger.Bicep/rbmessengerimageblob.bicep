param location string

resource storageaccount 'Microsoft.Storage/storageAccounts@2023-04-01' = {
  name: 'rbmessengerimageblob'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    publicNetworkAccess: 'Enabled'
    allowBlobPublicAccess: true
    accessTier: 'Hot'
  }
}

resource blobservice 'Microsoft.Storage/storageAccounts/blobServices@2023-04-01' = {
  name: 'default'
  parent: storageaccount
}

resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2019-06-01' = {
  name: 'avatar'
  parent: blobservice
  properties: {
    publicAccess: 'Blob'
    metadata: {}
  }
}

var connectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageaccount.name};AccountKey=${storageaccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'

output blobConnectionString string = connectionString
