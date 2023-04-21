// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName) =>       
           await azure.ResourceGroups.ContainAsync(resourceGroupName);
        

        public ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IResourceGroup> DeleteResourceGroupAsync(string reresourceGroupName)
        {
            throw new NotImplementedException();
        }
    }
}
