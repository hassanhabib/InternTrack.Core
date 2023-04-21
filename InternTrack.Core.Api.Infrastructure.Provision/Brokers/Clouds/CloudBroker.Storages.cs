// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Models.Storages;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<ISqlDatabase> CreateSqlDataBaseAsync(string sqlDatabaseName, ISqlServer sqlServer)
        {
            return await azure.SqlServers.Databases
                .Define(sqlDatabaseName)
                .WithExistingSqlServer(sqlServer)
                .CreateAsync();
        }

        public async ValueTask<ISqlServer> CreateSqlServerAsync(
            string sqlServerName,
            IResourceGroup resourceGroup)
        {
            return await azure.SqlServers
                .Define(name: sqlServerName)
                .WithRegion(region: Region.USWest2)
                .WithExistingResourceGroup(resourceGroup)
                .WithAdministratorLogin(adminName)
                .WithAdministratorPassword(adminAccess)
                .CreateAsync();
        }

        public SqlDatabaseAccess GetAdminAccess()
        {
            return new SqlDatabaseAccess
            {
                AdminName = adminName,
                AdminAccess = adminAccess
            };
        }
    }
}
