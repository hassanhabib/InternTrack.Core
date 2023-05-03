// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds;
using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Logging;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;
using InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

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

        public async ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCE-{environment}".ToUpper();
            loggingBroker.LogActivity(message: $"Provisioning {resourceGroupName}...");

            IResourceGroup resourceGroup =
                await cloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            loggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned");

            return resourceGroup;
        }

        public async ValueTask<IAppServicePlan> ProvisionPlanAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string planName = $"{projectName}-PLAN-{environment}".ToUpper();
            loggingBroker.LogActivity(message: $"Provisiong {planName}...");

            IAppServicePlan plan =
                await cloudBroker.CreatePlanAsync(
                    planName,
                    resourceGroup);

            loggingBroker.LogActivity(message: $"{plan} Provisioned");

            return plan;
        }

        public async ValueTask<ISqlServer> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string sqlServerName = $"{projectName}-dbserver-{environment}".ToLower();
            loggingBroker.LogActivity(message: $"Provisioning {sqlServerName}...");

            ISqlServer sqlServer =
                await cloudBroker.CreateSqlServerAsync(
                    sqlServerName,
                    resourceGroup);

            loggingBroker.LogActivity(message: $"{sqlServer} Provisioned)");

            return sqlServer;
        }

        public async ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectName,
            string environment,
            ISqlServer sqlServer)
        {
            string sqlDatabaseName = $"{projectName}-db-{environment}".ToLower();
            loggingBroker.LogActivity(message: $"Provisioning {sqlDatabaseName}");

            ISqlDatabase sqlDatabase =
                await cloudBroker.CreateSqlDataBaseAsync(
                    sqlDatabaseName,
                    sqlServer);

            loggingBroker.LogActivity(message: $"{sqlDatabaseName} Provisioned");

            return new SqlDatabase
            {
                Database = sqlDatabase,
                ConnectionString = GenerateConnectionString(sqlDatabase)
            };
        }

        public async ValueTask<IWebApp> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            IResourceGroup resourceGroup,
            IAppServicePlan appServicePlan)
        {
            string webAppName = $"{projectName}-{environment}".ToLower();
            loggingBroker.LogActivity(message: $"Provisioning {webAppName}");

            IWebApp webApp =
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

        private string GenerateConnectionString(ISqlDatabase sqlDatabase)
        {
            SqlDatabaseAccess sqlDatabaseAccess =
                cloudBroker.GetAdminAccess();

            return $"Server=tcp:{sqlDatabase.SqlServerName}.database.windows.net,1433;" +
                $"Initial Catalog={sqlDatabase.Name}" +
                $"User ID={sqlDatabaseAccess.AdminName}" +
                $"Password={sqlDatabaseAccess.AdminAccess}";
        }
    }
}
