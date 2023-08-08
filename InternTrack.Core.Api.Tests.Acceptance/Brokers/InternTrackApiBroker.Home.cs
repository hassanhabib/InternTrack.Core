using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternTrack.Core.Api.Tests.Acceptance.Brokers
{
    public partial class InternTrackApiBroker
    {
        private const string HomeRelativeUrl = "api/home";

        public async ValueTask<string> GetHomeMessage() =>
            await this.apiFactoryClient.GetContentStringAsync(HomeRelativeUrl);
    }
}
