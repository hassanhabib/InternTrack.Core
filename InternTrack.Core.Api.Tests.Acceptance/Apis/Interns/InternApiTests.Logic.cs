using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using Xunit;

namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Interns
{
    public partial class InternApiTests
    {
        [Fact]
        public async Task ShouldPostInternAsync()
        {
            //given
            Intern randomIntern = CreateRandomIntern();
            Intern inputIntern = randomIntern;
            Intern expectedIntern = inputIntern;

            //when
            await this.internTrackApiBroker
                .PostInternAsync(inputIntern);

            Intern actualIntern =
                await this.internTrackApiBroker
                    .GetInternByIdAsync(inputIntern.Id);

            //
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            await this.internTrackApiBroker
                .DeleteInternByIdAsync(inputIntern.Id);
        }

        [Fact]
        public async Task ShouldGetInternByIdAsync()
        {
            //given
            Intern randomIntern = await PostRandomInternAsync();
            Intern expectedIntern = randomIntern;

            //when
            Intern actualIntern =
                await this.internTrackApiBroker
                    .GetInternByIdAsync(randomIntern.Id);

            //then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            await this.internTrackApiBroker
                .DeleteInternByIdAsync(actualIntern.Id);
        }                
    }
}
