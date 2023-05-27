// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<SqlServerResource> CreateSqlServerAsync(
            string sqlServerName,
            ResourceGroupResource resourceGroup);

        ValueTask<SqlDatabaseResource> CreateSqlDataBaseAsync(
            string sqlDatabaseName,
            SqlServerResource sqlServer);

        SqlDatabaseAccess GetAdminAccess();
    }
}
