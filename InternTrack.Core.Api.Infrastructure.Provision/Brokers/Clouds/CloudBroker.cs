// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker : ICloudBroker
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tenantId;
        private readonly string adminName;
        private readonly string adminAccess;
        private readonly IAzure azure;

        public CloudBroker()
        {
            /*            clientId = Environment.GetEnvironmentVariable("AzureClientId");
                        clientSecret = Environment.GetEnvironmentVariable("AzureClientSecrect");
                        tenantId = Environment.GetEnvironmentVariable("AzureTenantId");
                        adminName = Environment.GetEnvironmentVariable("AzureAdminName");
                        adminAccess = Environment.GetEnvironmentVariable("AzureAdminAccess");
                        azure = AuthenticateAzure();*/
            clientId = "54f38412-c2fd-441c-aa0d-39515c554763";
            clientSecret = "_y28Q~yEUOA3PH4IfswtUIghThJG.aohwT2Pxdzm";
            adminName = "Pwongchaiya";
            adminAccess = "SecrectPassword2023";
            tenantId = "ebf2a41f-f65f-4bdb-8ce6-cc1b36518344";
            azure = AuthenticateAzure();
        }

        private IAzure AuthenticateAzure()
        {
            AzureCredentials credentials =
                SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(
                    clientId: clientId,
                    clientSecret: clientSecret,
                    tenantId: tenantId,
                    environment: AzureEnvironment.AzureGlobalCloud
                );

            return Azure.Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
        }
    }
}
