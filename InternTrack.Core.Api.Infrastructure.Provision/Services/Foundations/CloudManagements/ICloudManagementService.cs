// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace InternTrack.Core.Api.Infrastructure.Provision.Services.Foundations.CloudMangaments
{
    public interface ICloudManagementService
    {
        ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment
            );
    }
}
