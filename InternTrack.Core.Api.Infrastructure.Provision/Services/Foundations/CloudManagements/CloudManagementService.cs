// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds;
using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Logging;
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

        public CloudManagementService(ICloudBroker cloudBroker, ILoggingBroker loggingBroker)
        {
            CloudBroker = cloudBroker;
            LoggingBroker = loggingBroker;
        }

        public async ValueTask<IResourceGroup> ProvisionResourceGroupAsync(string projectName, string environment)
        {
            string resourceGroupName = $"{projectName}-RESOURCE-{environment}".ToUpper();

            LoggingBroker.LogActivity(message: $"Provisioning {resourceGroupName}...");

            IResourceGroup resourceGroup =
                await CloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            LoggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned");

            return resourceGroup;
        }

        public async ValueTask<IAppServicePlan> ProvisionPlanAsync(string projectName, string environment, IResourceGroup resourceGroup)
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

        public async ValueTask<ISqlServer> ProvisionSqlServerAsync(string projectName, string environment, IResourceGroup resourceGroup)
        {
            string sqlServerName = $"{projectName}-dbserver-{environment}".ToLower();
            
            LoggingBroker.LogActivity(message: $"Provisioning {projectName}...");

            ISqlServer sqlServer = 
                await CloudBroker.CreateSqlServerAsync( 
                    sqlServerName, 
                    resourceGroup);

            LoggingBroker.LogActivity(message: $"{sqlServerName} Provisioned)");

            return sqlServer;
        }
    }
}
