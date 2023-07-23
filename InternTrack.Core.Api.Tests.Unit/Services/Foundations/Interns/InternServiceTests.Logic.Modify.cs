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
        public async Task ShouldModifyInternAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern();
            Intern inputIntern = randomIntern;
            Intern StorageIntern = inputIntern.DeepClone();
            inputIntern.UpdatedDate = randomDate.AddMinutes(1);
            Intern updatedIntern = inputIntern;
            Intern expectedIntern = updatedIntern.DeepClone();

            Guid internId = inputIntern.Id;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(internId))
                    .ReturnsAsync(StorageIntern);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateInternAsync(inputIntern))
                    .ReturnsAsync(updatedIntern);

            // when
            Intern actualIntern = await this.internService.ModifyInternAsync(inputIntern);

            // then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(internId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateInternAsync(inputIntern),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
