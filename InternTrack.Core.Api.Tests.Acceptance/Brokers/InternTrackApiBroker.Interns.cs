using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Tests.Acceptance.Brokers
{
    public partial class InternTrackApiBroker
    {
        private const string InternRelativeString = "api/interns";

        public async ValueTask<Intern> PostInternAsync(Intern intern) =>
            await this.apiFactoryClient.PostContentAsync(InternRelativeString, intern);

        public async ValueTask<Intern> GetInternByIdAsync(Guid internId) =>
            await this.apiFactoryClient.GetContentAsync<Intern>($"{InternRelativeString}/{internId}");

        public async ValueTask<Intern> DeleteInternByIdAsync(Guid internId) =>
            await this.apiFactoryClient.DeleteContentAsync<Intern>($"{InternRelativeString}/{internId}");
    }
}
