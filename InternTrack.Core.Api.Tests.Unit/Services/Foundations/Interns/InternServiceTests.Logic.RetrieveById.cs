// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveInternById()
        {
            //given
            Intern randomIntern = CreateRandomIntern();
            Guid inputInternId = randomIntern.Id;
            Intern storageIntern = randomIntern;
            Intern expectedIntern = storageIntern;

            this.storageBrokerMock.Setup(broker => broker.SelectInternByIdAsync(inputInternId))
                .ReturnsAsync(storageIntern);

            //when
            Intern actualIntern =
                    await this.internService.RetrieveInternByIdAsync(inputInternId);

            //then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(inputInternId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }
    }
}