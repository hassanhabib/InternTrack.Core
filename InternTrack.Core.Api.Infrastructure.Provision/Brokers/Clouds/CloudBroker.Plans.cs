﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.AppService;
using Azure.ResourceManager.AppService.Models;
using Azure.ResourceManager.Resources;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<AppServicePlanResource> CreatePlanAsync(
            string planName,
            ResourceGroupResource resourceGroup)
        {
            var appServicePlanData =
                new AppServicePlanData(AzureLocation.WestUS3)
                {
                    Sku = new AppServiceSkuDescription()
                    {
                        Name = "F1",
                        Tier = "Free",
                        Size = "F1",
                        Family = "F",
                        Capacity = 0,
                    },
                    Kind = "app",
                };

            ArmOperation<AppServicePlanResource> plan = await resourceGroup
                .GetAppServicePlans()
                .CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    planName,
                    appServicePlanData);

            return plan.Value;
        }
    }
}
