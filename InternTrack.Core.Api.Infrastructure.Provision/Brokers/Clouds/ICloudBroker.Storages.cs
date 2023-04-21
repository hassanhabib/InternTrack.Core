// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<ISqlServer> CreateSqlServerAsync(
                string sqlServerName,
                IResourceGroup resourceGroup
            );

        ValueTask<ISqlServer> CreateSqlDataBaseAsync(
                string sqlDatabaseName,
                ISqlServer sqlServer
            );
    }
}
