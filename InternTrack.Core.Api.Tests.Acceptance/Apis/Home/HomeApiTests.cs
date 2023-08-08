using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Tests.Acceptance.Brokers;
using Xunit;

namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Interns
{
    [Collection(nameof(ApiTestCollection))]
    public class HomeApiTests
    {
        private readonly InternTrackApiBroker apiBroker;

        public HomeApiTests(InternTrackApiBroker apiBroker) =>
            this.apiBroker = apiBroker;

        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            // given
            string expectedMessage =
                "Thank you Mario! But the princess is in another castle!";

            // when
            string actualMessage =
                await this.apiBroker.GetHomeMessage();

            // then
            actualMessage.Should().Be(expectedMessage);
        }
    }
}
