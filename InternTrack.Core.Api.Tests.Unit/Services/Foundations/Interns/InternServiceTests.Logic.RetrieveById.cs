// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using InternTrack.Core.Api.Models.Interns;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveInternByIdAsync()
        {
            // given
            Guid randomInternId = Guid.NewGuid();
            Guid inputInternId = randomInternId;
            Intern randomIntern = CreateRandomIntern();
            Intern storageIntern = randomIntern;
            Intern expectedIntern = storageIntern.DeepClone();

            this.storageBrokerMock.Setup(broker => 
                broker.SelectInternByIdAsync(randomInternId))
                    .ReturnsAsync(storageIntern);

            // when
            Intern actualIntern =
                    await this.internService.RetrieveInternByIdAsync(inputInternId);

            // then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(inputInternId), 
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}