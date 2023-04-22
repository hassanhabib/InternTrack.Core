﻿// -------------------------------------------------------
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
        private readonly ICloudBroker CloudBroker;
        private readonly ILoggingBroker LoggingBroker;

        public CloudManagementService()
        {
            CloudBroker = new CloudBroker();
            LoggingBroker = new LoggingBroker();
        }

        public async ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCE-{environment}".ToUpper();

            LoggingBroker.LogActivity(message: $"Provisioning {resourceGroupName}...");

            IResourceGroup resourceGroup =
                await CloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            LoggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned");

            return resourceGroup;
        }

        public async ValueTask<IAppServicePlan> ProvisionPlanAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string planName = $"{projectName}-PLAN-{environment}".ToUpper();

            LoggingBroker.LogActivity(message: $"Provisiong {planName}...");

            IAppServicePlan plan =
                await CloudBroker.CreatePlanAsync(
                    planName,
                    resourceGroup);

            LoggingBroker.LogActivity(message: $"{plan} Provisioned");

            return plan;
        }

        public async ValueTask<ISqlServer> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup)
        {
            string sqlServerName = $"{projectName}-dbserver-{environment}".ToLower();

            LoggingBroker.LogActivity(message: $"Provisioning {sqlServerName}...");

            ISqlServer sqlServer =
                await CloudBroker.CreateSqlServerAsync(
                    sqlServerName,
                    resourceGroup);

            LoggingBroker.LogActivity(message: $"{sqlServer} Provisioned)");

            return sqlServer;
        }

        public async ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectName,
            string environment,
            ISqlServer sqlServer)
        {
            string sqlDatabaseName = $"{projectName}-db-{environment}".ToLower();

            LoggingBroker.LogActivity(message: $"Provisioning {sqlDatabaseName}");

            ISqlDatabase sqlDatabase =
                await CloudBroker.CreateSqlDataBaseAsync(
                    sqlDatabaseName,
                    sqlServer
                    );

            LoggingBroker.LogActivity(message: $"{sqlDatabaseName} Provisioned");

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

            LoggingBroker.LogActivity(message: $"Provisioning {webAppName}");

            IWebApp webApp =
                await CloudBroker.CreateWebAppAsync(
                        webAppName,
                        databaseConnectionString,
                        appServicePlan,
                        resourceGroup
                    );

            LoggingBroker.LogActivity(message: $"{webAppName} Provisioned");

            return webApp;
        }

        public async ValueTask DeprovisionResourceGroupAsync(string projectName, string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCE-{environment}".ToUpper();
            bool isResourceGroupExist =
                await CloudBroker.CheckResourceGroupExistAsync(
                    resourceGroupName);

            if (isResourceGroupExist)
            {
                LoggingBroker.LogActivity(
                    message: $"Deprovisioning {resourceGroupName}...");

                await CloudBroker.DeleteResourceGroupAsync(
                    resourceGroupName
                );

                LoggingBroker.LogActivity(
                    message: $"{resourceGroupName} Deprovisioned");
            }
            else
            {
                LoggingBroker.LogActivity(
                    message: $"Resource group name {resourceGroupName} doesn't exist.");
            }
        }

        private string GenerateConnectionString(ISqlDatabase sqlDatabase)
        {
            SqlDatabaseAccess sqlDatabaseAccess =
                CloudBroker.GetAdminAccess();

            return $"Server=tcp:{sqlDatabase.SqlServerName}.database.windows.net,1433;" +
                $"Initial Catalog={sqlDatabase.Name}" +
                $"User ID={sqlDatabaseAccess.AdminName}" +
                $"Password={sqlDatabaseAccess.AdminAccess}";
        }
    }
}