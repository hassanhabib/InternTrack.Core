// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds;
using InternTrack.Core.Api.Infrastructure.Provision.Brokers.Logging;
using InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments;
using Microsoft.Azure.Management.ResourceManager.Fluent;

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

            LoggingBroker.LogActivity(message: $"Provisioning {resourceGroupName}");

            IResourceGroup resourceGroup =
                await CloudBroker.CreateResourceGroupAsync(
                    resourceGroupName);

            LoggingBroker.LogActivity(message: $"{resourceGroupName} Provisioned");

            return resourceGroup;
        }
    }
}
