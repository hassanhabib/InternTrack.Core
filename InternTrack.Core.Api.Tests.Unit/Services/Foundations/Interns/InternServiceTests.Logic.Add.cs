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
        private async Task ShouldAddInternAsync()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern(dateTime);
            randomIntern.UpdatedBy = randomIntern.CreatedBy;
            randomIntern.UpdatedDate = randomIntern.CreatedDate;
            Intern inputIntern = randomIntern;
            Intern storageIntern = inputIntern;
            Intern expectedIntern = storageIntern.DeepClone();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertInternAsync(inputIntern))
                    .ReturnsAsync(storageIntern);

            //when
            Intern actualIntern =
                await this.internService.AddInternAsync(inputIntern);

            //then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(inputIntern),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
