param location string

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'MyMessengerPlan'
  location: location
  sku: {
    name: 'F1'
    capacity: 1
  }
}

output appServicePlanId string = appServicePlan.id
