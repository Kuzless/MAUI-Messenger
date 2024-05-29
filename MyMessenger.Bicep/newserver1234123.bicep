param location string
var login = 'Kuzless'
var pass = '123QwE321'

resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' ={
  name: 'newserver1234123'
  location: location
  properties: {
    administratorLogin: login
    administratorLoginPassword: pass
    publicNetworkAccess: 'Enabled'
  }
}

resource SQLAllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  name: 'AllowAllWindowsAzureIps'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

resource SQLAllConnectionsAllowed 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  name: 'AllConnectionsAllowed'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '255.255.255.255'
  }
}

resource sqlServerDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: 'Messenger'
  location: location
  sku: {
    name: 'GP_S_Gen5'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 2
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 34359738368
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    autoPauseDelay: 60
    requestedBackupStorageRedundancy: 'Local'
    isLedgerOn: false
    useFreeLimit: true
    freeLimitExhaustionBehavior: 'AutoPause'
    availabilityZone: 'NoPreference'
  }
}

var serverName = sqlServer.name
var dbName = sqlServerDatabase.name
var adminLogin = login
var adminPassword = pass
var fullyQualifiedDomainName = reference(serverName).fullyQualifiedDomainName

var connectionString = 'Server=tcp:${fullyQualifiedDomainName},1433;Initial Catalog=${dbName};Persist Security Info=False;User ID=${adminLogin};Password=${adminPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

output dbConnectionString string = connectionString
