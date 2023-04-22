// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Configurations;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Configurations;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;
using InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements;
using InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Services.Processings.CloudManagements
{
    public class CloudManagementProcessingService : ICloudManagementProcessingService
    {
        private readonly ICloudManagementService CloudManagementService;
        private readonly IConfigurationBroker ConfigurationBroker;

        public CloudManagementProcessingService()
        {
            CloudManagementService = new CloudManagementService();
            ConfigurationBroker = new ConfigurationBroker();
        }

        public async ValueTask ProcessAsync()
        {
            CloudMangamentConfiguration cloudMangamentConfiguration =
                ConfigurationBroker.GetConfigurations();

            await ProvisionAsync(
                    projectName: cloudMangamentConfiguration.ProjectName,
                    cloudAction: cloudMangamentConfiguration.Up);

            await DeprovisionAsync(
                    projectName: cloudMangamentConfiguration.ProjectName,
                    cloudAction: cloudMangamentConfiguration.Down);
        }

        private async ValueTask ProvisionAsync(
                string projectName,
                CloudAction cloudAction)
        {
            List<string> environment = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environment)
            {
                IResourceGroup resourceGroup = await CloudManagementService
                    .ProvisionResourceGroupAsync(
                        projectName,
                        environmentName);

                IAppServicePlan appServicePlan = await CloudManagementService
                    .ProvisionPlanAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                ISqlServer sqlServer = await CloudManagementService
                    .ProvisionSqlServerAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                SqlDatabase sqlDatabase = await CloudManagementService
                    .ProvisionSqlDatabaseAsync(
                        projectName,
                        environmentName,
                        sqlServer);

                IWebApp webApp = await CloudManagementService
                    .ProvisionWebAppAsync(
                        projectName,
                        environmentName,
                        sqlDatabase.ConnectionString,
                        resourceGroup,
                        appServicePlan);
            }
        }

        private async ValueTask DeprovisionAsync(
                string projectName,
                CloudAction cloudAction
            )
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                await CloudManagementService.DeprovisionResourceGroupAsync(projectName, environmentName);
            }
        }

        private static List<string> RetrieveEnvironments(CloudAction cloudAction) =>
            cloudAction?.Environments ?? new List<string>();
    }
}
