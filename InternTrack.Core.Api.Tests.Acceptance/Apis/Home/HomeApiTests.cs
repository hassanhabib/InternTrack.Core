// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Tests.Acceptance.Brokers;
using Xunit;

namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Home
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTests
    {
        private readonly InternTrackApiBroker apiBroker;

        public HomeApiTests(InternTrackApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        [Fact]
        private async Task ShouldReturnHomeMessageAsync()
        {
            // given
            string expectedMessage =
                "Thank you Mario! But the princess is in another castle!";

            // when
            string actualMessage =
                await apiBroker.GetHomeMessage();

            // then
            actualMessage.Should().Be(expectedMessage);
        }
    }
}
