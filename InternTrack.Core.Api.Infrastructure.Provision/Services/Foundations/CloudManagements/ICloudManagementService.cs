// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments
{
    public interface ICloudManagementService
    {
        ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment
            );

        ValueTask<IAppServicePlan> ProvisionPlanAsync(
                string projectName,
                string environment,
                IResourceGroup resourceGroup
            );

        ValueTask<ISqlServer> ProvisionSqlServerAsync(
                string projectName,
                string environment,
                IResourceGroup resourceGroup
            );

        ValueTask<ISqlDatabase> ProvisionSqlDatabaseAsync(
                string projectName,
                string environment,
                ISqlServer sqlServer
            );
    }
}
