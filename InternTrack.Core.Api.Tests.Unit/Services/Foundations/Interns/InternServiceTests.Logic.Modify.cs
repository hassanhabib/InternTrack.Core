using FluentAssertions;
using Force.DeepCloner;
using InternTrack.Core.Api.Models.Interns;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldModifyInternAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern();
            Intern inputIntern = randomIntern;
            Intern afterUpdateStorageIntern = inputIntern;
            Intern expectedIntern = afterUpdateStorageIntern;
            Intern beforeUpdateStorageIntern = randomIntern.DeepClone();
            inputIntern.UpdatedDate = randomDate;
            Guid internId = inputIntern.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(internId))
                    .ReturnsAsync(beforeUpdateStorageIntern);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateInternAsync(inputIntern))
                    .ReturnsAsync(afterUpdateStorageIntern);

            // when
            Intern actualIntern = await this.internService.ModifyInternAsync(inputIntern);

            // then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(internId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateInternAsync(inputIntern),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
