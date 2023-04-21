// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker : ICloudBroker
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string tenantId;
        private readonly IAzure azure;

        public CloudBroker()
        {
            clientId = Environment.GetEnvironmentVariable("AzureClientId");
            clientSecret = Environment.GetEnvironmentVariable("AzureClientSecrect");
            tenantId = Environment.GetEnvironmentVariable("AzureTenantId");
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
                .WithLogLevel(Microsoft.Azure.Management.ResourceManager.Fluent.Core.HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
        }
    }
}
