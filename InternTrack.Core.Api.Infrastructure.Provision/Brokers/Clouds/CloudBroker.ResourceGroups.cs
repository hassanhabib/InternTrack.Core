// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.Core;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using Azure;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<ResourceGroupResource> CreateResourceGroupAsync(
            string resourceGroupName)
        {
            SubscriptionResource subscription = await client
                .GetDefaultSubscriptionAsync();

            ResourceGroupCollection resourceGroups = subscription
                .GetResourceGroups();

            ResourceGroupData resourceGroupData = new ResourceGroupData(
                AzureLocation.WestUS3);

            ArmOperation<ResourceGroupResource> operation = await resourceGroups
                .CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    resourceGroupName,
                    resourceGroupData);

            return operation.Value;
        }

        public async ValueTask<bool> CheckResourceGroupExistAsync(
            string resourceGroupName)
        {
            SubscriptionResource subscription = await client
                .GetDefaultSubscriptionAsync();

            return await subscription
                .GetResourceGroups()
                .ExistsAsync(resourceGroupName);
        }

        public async ValueTask DeleteResourceGroupAsync(ResourceGroupResource reresourceGroupName) =>
            await client
                .GetResourceGroupResource(reresourceGroupName.Id)
                .DeleteAsync(WaitUntil.Completed);
    }
}
