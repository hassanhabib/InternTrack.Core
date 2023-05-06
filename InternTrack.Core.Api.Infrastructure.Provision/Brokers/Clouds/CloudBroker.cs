// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Azure.Identity;
using Azure.ResourceManager;

/* 
Environment Variables naming convention:
AZURE_CLIENT_ID
AZURE_CLIENT_SECRET
AZURE_TENANT_ID
AZURE_ADMIN_NAME
AZURE_ADMIN_Password
 */

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker : ICloudBroker
    {
        private readonly string adminName;
        private readonly string adminPassword;
        private readonly ArmClient client;

        public CloudBroker()
        {
            this.adminName = Environment.GetEnvironmentVariable("Azure_Admin_Name");
            this.adminPassword = Environment.GetEnvironmentVariable("Azure_Admin_Password");
            this.client = new ArmClient(new EnvironmentCredential());
        }
    }
}
