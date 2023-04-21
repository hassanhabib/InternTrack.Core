using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker : ICloudBroker
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string clientTenantId;

       private IAzure AuthenticateAzure()
        {
            AzureCredentials credentials = 
                SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(
                    clientId: null,
                    clientSecret: null,
                    tenantId: null,
                    environment: AzureEnvironment.AzureGlobalCloud
                );

            return Azure.Configure()
                .WithLogLevel(Microsoft.Azure.Management.ResourceManager.Fluent.Core.HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(credentials)
                .WithDefaultSubscription();
        }
    }
}
