// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using Azure.ResourceManager.Sql.Models;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<SqlDatabaseResource> CreateSqlDataBaseAsync(
            string sqlDatabaseName,
            SqlServerResource sqlServer)
        {
            SqlDatabaseCollection sqlDbs = sqlServer
                .GetSqlDatabases();

            var sqlDbData =
                new SqlDatabaseData(AzureLocation.WestUS3)
                {
                    Sku = new SqlSku("S0")
                    {
                        Tier = "Standard",
                    },
                    CreateMode = SqlDatabaseCreateMode.Default,
                };

            ArmOperation<SqlDatabaseResource> database = await sqlDbs
                .CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    sqlDatabaseName,
                    sqlDbData);

            return database.Value;
        }

        public async ValueTask<SqlServerResource> CreateSqlServerAsync(
            string sqlServerName,
            ResourceGroupResource resourceGroup)
        {
            SqlServerCollection sqlServers = resourceGroup
                .GetSqlServers();

            AzureLocation location = AzureLocation.WestUS3;

            var sqlServerData = new SqlServerData(location)
            {
                AdministratorLogin = GetAdminAccess().AdminName,
                AdministratorLoginPassword = GetAdminAccess().AdminAccess,
            };

            ArmOperation<SqlServerResource> server = await sqlServers
                .CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    sqlServerName,
                    sqlServerData);

            return server.Value;
        }

        public SqlDatabaseAccess GetAdminAccess()
        {
            return new SqlDatabaseAccess
            {
                AdminName = adminName,
                AdminAccess = adminPassword
            };
        }
    }
}
