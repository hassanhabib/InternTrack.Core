// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using FluentAssertions;
using Xunit;
namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Home
{
    public partial class HomeApiTests
    {
        [Fact]
        public async Task ShouldReturnHomeMessageAsync()
        {
            // Given
            string expectedMessage = 
                "Thank you, Mario. But the princess is in another castle!";
            // When
            string actualMessage = 
                await this.internTrackApiBroker.GetHomeMessageAsync();
            // Then
            actualMessage.Should().BeEquivalentTo(expectedMessage);
        }
    }
}
