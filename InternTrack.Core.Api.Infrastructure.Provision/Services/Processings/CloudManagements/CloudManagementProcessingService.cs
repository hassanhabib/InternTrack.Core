// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Sql;
using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Configurations;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Configurations;
using InternTrack.Core.Api.Infrastructure.Provision.Models.Storages;
using InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudManagements;

namespace InternTrack.Core.Api.Infrastructure.Provision.Services.Processings.CloudManagements
{
    public class CloudManagementProcessingService : ICloudManagementProcessingService
    {
        private readonly ICloudManagementService cloudManagementService;
        private readonly IConfigurationBroker configurationBroker;

        public CloudManagementProcessingService()
        {
            this.cloudManagementService = new CloudManagementService();
            this.configurationBroker = new ConfigurationBroker();
        }

        public async ValueTask ProcessAsync()
        {
            CloudMangamentConfiguration cloudMangamentConfiguration =
                configurationBroker.GetConfigurations();

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
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                ResourceGroupResource resourceGroup = await cloudManagementService
                    .ProvisionResourceGroupAsync(
                        projectName,
                        environmentName);

                AppServicePlanResource appServicePlan = await cloudManagementService
                    .ProvisionPlanAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                SqlServerResource sqlServer = await cloudManagementService
                    .ProvisionSqlServerAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                SqlDatabase sqlDatabase = await cloudManagementService
                    .ProvisionSqlDatabaseAsync(
                        projectName,
                        environmentName,
                        sqlServer);

                WebSiteResource webApp = await cloudManagementService
                    .ProvisionWebAppAsync(
                        projectName,
                        environmentName,
                        sqlDatabase.ConnectionString,
                        resourceGroup,
                        appServicePlan);

                ApplicationInsightsComponentResource applicationInsight =
                    await cloudManagementService
                        .ProvisionApplicationInsightComponentAsync(
                            projectName,
                            environmentName,
                            resourceGroup);
            }
        }

        private async ValueTask DeprovisionAsync(
            string projectName,
            CloudAction cloudAction)
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                await cloudManagementService.DeprovisionResourceGroupAsync(
                    projectName,
                    environmentName);
            }
        }

        private static List<string> RetrieveEnvironments(CloudAction cloudAction) =>
            cloudAction?.Environments ?? new List<string>();
    }
}
