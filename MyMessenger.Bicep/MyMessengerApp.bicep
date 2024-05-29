param location string
param appServicePlanId string

resource webApplication 'Microsoft.Web/sites@2021-01-15' = {
  name: 'MyMessengerApp'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlanId
  }
}

resource webApi 'Microsoft.ApiManagement/service@2023-05-01-preview' = {
  name: 'MyMessengerapi'
  location: location
  sku:{
    capacity: 0
    name: 'Consumption'
  }
  properties: {
    virtualNetworkType: 'None'
    publisherEmail: 'mykhailo.riazhskyi@hneu.net'
    publisherName: 'Михайло Ряжський'
  }
}

resource api 'Microsoft.ApiManagement/service/apis@2023-05-01-preview' = {
  name: webApplication.name
  parent: webApi
  properties: {
    displayName: webApplication.name
    path: 'api'
    protocols: [
      'https'
    ]
    serviceUrl: 'https://${webApplication.name}.azurewebsites.net'
  }
}

output webAppId string = webApplication.identity.principalId
