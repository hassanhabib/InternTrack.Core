// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName) =>
           await azure.ResourceGroups.ContainAsync(resourceGroupName);

        public async ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName)
        {
            return await azure.ResourceGroups
                .Define(name: resourceGroupName)
                .WithRegion(region: Region.USWest3)
                .CreateAsync();
        }

        public async ValueTask DeleteResourceGroupAsync(string reresourceGroupName) =>
            await azure.ResourceGroups.DeleteByNameAsync(reresourceGroupName);
    }
}
