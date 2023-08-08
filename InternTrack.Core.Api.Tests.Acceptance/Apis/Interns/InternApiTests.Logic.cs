using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using RESTFulSense.Exceptions;
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

        [Fact]
        public async Task ShouldGetAllInternsAsync()
        {
            //given
            List<Intern> randomInterns = 
                await CreateRandomPostedInternsAsync();

            List<Intern> expectedInterns = randomInterns;

            //when
            List<Intern> actualInterns =
                await this.internTrackApiBroker.GetAllInternsAsync();

            //then
            foreach (Intern expectedIntern in expectedInterns)
            {
                Intern actualIntern = 
                    actualInterns.Single(
                        intern => intern.Id == expectedIntern.Id);

                actualIntern.Should().BeEquivalentTo(expectedIntern);

                await this.internTrackApiBroker
                    .DeleteInternByIdAsync(actualIntern.Id);
            }
        }

        [Fact]
        public async Task ShouldPutInternAsync()
        {
            //given
            Intern randomIntern = await PostRandomInternAsync();
            Intern modifiedIntern = UpdateRandomIntern(randomIntern);

            //when
            await this.internTrackApiBroker.PutInternAsync(modifiedIntern);

            Intern actualIntern =
                await this.internTrackApiBroker.GetInternByIdAsync(randomIntern.Id);

            //then
            actualIntern.Should().BeEquivalentTo(modifiedIntern);
            await this.internTrackApiBroker.DeleteInternByIdAsync(actualIntern.Id);
        }

        [Fact]
        public async Task ShouldDeleteInternAsync()
        {
            //given
            Intern randomIntern = await PostRandomInternAsync();
            Intern inputIntern = randomIntern;
            Intern expectedIntern = inputIntern;

            //when
            Intern deletedIntern =
                await this.internTrackApiBroker
                    .DeleteInternByIdAsync(inputIntern.Id);

            ValueTask<Intern> getInternByIdTask =
                this.internTrackApiBroker
                    .GetInternByIdAsync(inputIntern.Id);

            //then
            deletedIntern.Should().BeEquivalentTo(expectedIntern);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
                getInternByIdTask.AsTask());

        }
    }
}
