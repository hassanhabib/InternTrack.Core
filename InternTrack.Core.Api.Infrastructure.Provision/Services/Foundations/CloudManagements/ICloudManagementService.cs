// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments
{
    public interface ICloudManagementService
    {
        ValueTask<ResourceGroupResource> ProvisionResourceGroupAsync(
            string projectName,
            string environment);

        ValueTask<AppServicePlanResource> ProvisionPlanAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup);

        ValueTask<SqlServerResource> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup);

        ValueTask<SqlDatabaseResource> ProvisionSqlDatabaseAsync(
            string projectName,
            string environment,
            ISqlServer sqlServer);

        ValueTask<WebSiteResource> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            ResourceGroupResource resourceGroup,
            IAppServicePlan appServicePlan);

        ValueTask DeprovisionResourceGroupAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup);
    }
}
