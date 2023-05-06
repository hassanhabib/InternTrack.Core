// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.Resources;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<ResourceGroupResource> CreateResourceGroupAsync(string resourceGroupName);
        ValueTask DeleteResourceGroupAsync(ResourceGroupResource reresourceGroupName);
        ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName);
    }
}
