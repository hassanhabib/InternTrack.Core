using FluentAssertions;


namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Home
{

    public partial class HomeApiTests
    {

        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            string expectedMessage = "Thank you, Mario. But the princess is in another castler!";
            string actualMessage = await this.internTrackApiBroker.GetHomeMessageAsync();
            actualMessage.Should().BeEquivalentTo(expectedMessage);
        }

    }
}
