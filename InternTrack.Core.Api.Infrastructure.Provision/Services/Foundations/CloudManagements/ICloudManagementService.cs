// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments
{
    public interface ICloudManagementService
    {
        ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment);

        ValueTask<IAppServicePlan> ProvisionPlanAsync(
                string projectName,
                string environment,
                IResourceGroup resourceGroup);

        ValueTask<ISqlServer> ProvisionSqlServerAsync(
                string projectName,
                string environment,
                IResourceGroup resourceGroup);

        ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
                string projectName,
                string environment,
                ISqlServer sqlServer);

        ValueTask<IWebApp> ProvisionWebAppAsync(
                string projectName,
                string environment,
                string databaseConnectionString,
                IResourceGroup resourceGroup,
                IAppServicePlan appServicePlan);

        ValueTask DeprovisionResourceGroupAsync(
                string projectName,
                string environment);
    }
}
