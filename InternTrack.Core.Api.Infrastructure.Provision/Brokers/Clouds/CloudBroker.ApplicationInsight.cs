// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.Core;
using Azure.ResourceManager.ApplicationInsights.Models;
using Azure.ResourceManager.ApplicationInsights;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager;
using Azure;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<ApplicationInsightsComponentResource> CreateApplicationInsightComponentAsync(
            string componentName,
            ResourceGroupResource resourceGroup)
        {
            ApplicationInsightsComponentCollection appInsightCollection = 
                resourceGroup.GetApplicationInsightsComponents();

            var appInsightData = new ApplicationInsightsComponentData(AzureLocation.WestUS, "web")
            {
                ApplicationType = ApplicationType.Web,
                FlowType = FlowType.Bluefield,
                RequestSource = RequestSource.Rest
            };

            ArmOperation<ApplicationInsightsComponentResource> appInsightComponent =
                await appInsightCollection.CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    componentName,
                    appInsightData);

            return appInsightComponent.Value;
        }
    }
}
