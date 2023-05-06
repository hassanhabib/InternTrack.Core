// -------------------------------------------------------
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
        public async ValueTask<WebSiteResource> CreateWebAppAsync(
            string webAppName,
            string databaseConnectionString,
            AppServicePlanResource plan,
            ResourceGroupResource resourceGroup)
        {
            WebSiteCollection webSites = resourceGroup
                .GetWebSites();

            WebSiteData webSiteData =
                new WebSiteData(AzureLocation.WestUS3)
                {
                    AppServicePlanId = plan.Id,

                    SiteConfig = new SiteConfigProperties()
                    {
                        ConnectionStrings =
                        {
                            new ConnStringInfo()
                            {
                                Name = "DefaultConnection",

                                ConnectionString =
                                    databaseConnectionString,

                                ConnectionStringType =
                                    ConnectionStringType.SqlAzure
                            }
                        },

                        NetFrameworkVersion = "v7.0"
                    }
                };

            ArmOperation<WebSiteResource> webApp = await webSites
                .CreateOrUpdateAsync(
                    WaitUntil.Completed,
                    webAppName,
                    webSiteData);

            return webApp.Value;
        }
    }
}
