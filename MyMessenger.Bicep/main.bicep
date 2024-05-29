param location string = deployment().location
targetScope = 'subscription'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'Messenger'
  location: location
}

module appServicePlan './MyMessengerPlan.bicep' = {
  scope: resourceGroup
  name: 'MyMessengerPlan'
  params: {
    location: resourceGroup.location
  }
}

module webApplication './MyMessengerApp.bicep' = {
  scope: resourceGroup
  name: 'MyMessengerApp'
  params: {
    location: resourceGroup.location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
  }
}

module storageAccount './rbmessengerimageblob.bicep' = {
  scope: resourceGroup
  name: 'rbmessengerimageblob'
  params: {
    location: resourceGroup.location
  }
}

module keyVault './RBMessengerKeyvault.bicep' = {
  scope: resourceGroup
  name: 'RBMessengerKeyvault'
  params: {
    location: resourceGroup.location
    webAppId: webApplication.outputs.webAppId
    dbConStr: dataBase.outputs.dbConnectionString
    blobConStr: storageAccount.outputs.blobConnectionString
  }
}

module dataBase './newserver1234123.bicep' = {
  scope: resourceGroup
  name: 'newserver1234123'
  params: {
    location: resourceGroup.location
  }
}
