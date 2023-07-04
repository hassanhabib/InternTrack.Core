// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds;
using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Logging;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;
using InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments;

namespace InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements
{
    public class CloudManagementService : ICloudManagementService
    {
        private readonly ICloudBroker cloudBroker;
        private readonly ILoggingBroker loggingBroker;

        public CloudManagementService()
        {
            this.cloudBroker = new CloudBroker();
            this.loggingBroker = new LoggingBroker();
        }

        public async ValueTask<ResourceGroupResource> ProvisionResourceGroupAsync(
            string projectName,
            string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCE-{environment}".ToUpper();
            loggingBroker.LogActivity(message: $"Provisioning {resourceGroupName}...");

            ResourceGroupResource resourceGroup =
                await cloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            loggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned");

            return resourceGroup;
        }

        public async ValueTask<AppServicePlanResource> ProvisionPlanAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup)
        {
            string planName = $"{projectName}-PLAN-{environment}".ToUpper();
            loggingBroker.LogActivity(message: $"Provisiong {planName}...");

            AppServicePlanResource plan =
                await cloudBroker.CreatePlanAsync(
                    planName,
                    resourceGroup);

            loggingBroker.LogActivity(message: $"{plan} Provisioned");

            return plan;
        }

        public async ValueTask<SqlServerResource> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup)
        {
            string sqlServerName = $"{projectName}-dbserver-{environment}".ToLower();
            loggingBroker.LogActivity(message: $"Provisioning {sqlServerName}...");

            SqlServerResource sqlServer =
                await cloudBroker.CreateSqlServerAsync(
                    sqlServerName,
                    resourceGroup);

            loggingBroker.LogActivity(message: $"{sqlServer} Provisioned)");

            return sqlServer;
        }

        public async ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectName,
            string environment,
            SqlServerResource sqlServer)
        {
            string sqlDatabaseName = $"{projectName}-db-{environment}".ToLower();
            loggingBroker.LogActivity(message: $"Provisioning {sqlDatabaseName}");

            SqlDatabaseResource sqlDatabase =
                await cloudBroker.CreateSqlDataBaseAsync(
                    sqlDatabaseName,
                    sqlServer);

            loggingBroker.LogActivity(message: $"{sqlDatabaseName} Provisioned");

            return new SqlDatabase
            {
                Database = sqlDatabase,
                ConnectionString = GenerateConnectionString(sqlDatabase, sqlServer)
            };
        }

        public async ValueTask<WebSiteResource> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            ResourceGroupResource resourceGroup,
            AppServicePlanResource appServicePlan)
        {
            string webAppName = $"{projectName}-{environment}".ToLower();
            loggingBroker.LogActivity(message: $"Provisioning {webAppName}");

            WebSiteResource webApp =
                await cloudBroker.CreateWebAppAsync(
                    webAppName,
                    databaseConnectionString,
                    appServicePlan,
                    resourceGroup);

            loggingBroker.LogActivity(message: $"{webAppName} Provisioned");

            return webApp;
        }

        public async ValueTask DeprovisionResourceGroupAsync(
            string projectName,
            string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCE-{environment}".ToUpper();

            bool isResourceGroupExist =
                await cloudBroker.CheckResourceGroupExistAsync(
                    resourceGroupName);

            if (isResourceGroupExist)
            {
                loggingBroker.LogActivity(message: $"Deprovisioning {resourceGroupName}...");
                await cloudBroker.DeleteResourceGroupAsync(resourceGroupName);
                loggingBroker.LogActivity(message: $"{resourceGroupName} Deprovisioned");
            }
            else
            {
                loggingBroker.LogActivity(
                    message: $"Resource group name {resourceGroupName} doesn't exist.");
            }
        }

        private string GenerateConnectionString(
            SqlDatabaseResource sqlDatabase,
            SqlServerResource sqlServer)
        {
            SqlDatabaseAccess sqlDatabaseAccess =
                cloudBroker.GetAdminAccess();

            return $"Server=tcp:{sqlDatabase.Data.Name}.database.windows.net,1433;" +
                $"Initial Catalog={sqlServer.Data.Name}" +
                $"User ID={sqlDatabaseAccess.AdminName}" +
                $"Password={sqlDatabaseAccess.AdminAccess}";
        }

        public async ValueTask<ApplicationInsightsComponentResource> ProvisionApplicationInsightComponentAsync(
            string projectName,
            string environment,
            ResourceGroupResource resourceGroup)
        {
            string applicationInsightComponentName = $"{projectName}-Application-Insight-{environment}".ToLower();
            loggingBroker.LogActivity(message: $"Provisioning {applicationInsightComponentName}");

            ApplicationInsightsComponentResource applicationInsight =
                await cloudBroker.CreateApplicationInsightComponentAsync(
                    applicationInsightComponentName,
                    resourceGroup);

            loggingBroker.LogActivity(message: $"{applicationInsightComponentName} Provisioned");

            return applicationInsight;
        }
    }
}
