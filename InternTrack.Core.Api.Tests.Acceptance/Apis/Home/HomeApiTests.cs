// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Tests.Acceptance.Brokers;
using Xunit;

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
