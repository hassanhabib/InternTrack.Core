// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.Resources;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<ApplicationInsightsComponentResource> CreateApplicationInsightComponentAsync(
            string componentName,
            ResourceGroupResource resourceGroup);
    }
}
