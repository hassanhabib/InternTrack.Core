// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;

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

        ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectName,
            string environment,
            SqlServerResource sqlServer);

        ValueTask<WebSiteResource> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            ResourceGroupResource resourceGroup,
            AppServicePlanResource appServicePlan);

        ValueTask DeprovisionResourceGroupAsync(
            string projectName,
            string environment);

        ValueTask<ApplicationInsightsComponentResource> ProvisionApplicationInsightComponentAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup);
    }
}
