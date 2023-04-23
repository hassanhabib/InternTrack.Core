using InternTrack.Core.Api.Tests.Acceptance.Brokers;

namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Home
{
    [Collection(nameof(ApiTestCollection))]
    public partial class HomeApiTests
    {
        private readonly InternTrackApiBroker internTrackApiBroker;
        public HomeApiTests(InternTrackApiBroker internTrackApiBroker) =>
            this.internTrackApiBroker = internTrackApiBroker;

    }
}
