// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenInternIsNullAndLogItAsync()
        {
            // given
            Intern nullIntern = null;

            var nullInternException =
                new NullInternException();

            var expectedInternValidationException =
                new InternValidationException(nullInternException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(nullIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenInternIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidIntern = new Intern
            {
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText,
            };

            var invalidInternException = new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.FirstName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.MiddleName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.LastName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.Email),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.PhoneNumber),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.Status),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: new String[] { "Date is required", "Date is the same as CreatedDate" });

            invalidInternException.AddData(
                key: nameof(Intern.CreatedDate),
                values: "Date is required");

            invalidInternException.AddData(
                key: nameof(Intern.JoinDate),
                values: "Date is required");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedBy),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.CreatedBy),
                values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(GetRandomDateTimeOffset());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCreatedAndUpdatedDatesAreSameAndLogItAsync()
        {
            // given
            Intern randomIntern = CreateRandomIntern();
            DateTimeOffset sameDate = randomIntern.CreatedDate;
            Intern invalidIntern = randomIntern;
            invalidIntern.CreatedDate = sameDate;
            invalidIntern.UpdatedDate = sameDate;
            var invalidInternException = new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: $"Date is the same as {nameof(Intern.CreatedDate)}");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(sameDate);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            // then
            await Assert.ThrowsAsync<InternValidationException>(() =>
                modifyInternTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern(randomDateTime.AddMinutes(minutesBeforeOrAfter));
            Intern invalidIntern = randomIntern;
            invalidIntern.UpdatedBy = invalidIntern.CreatedBy;
            invalidIntern.UpdatedDate = invalidIntern.UpdatedDate.AddMinutes(minutesBeforeOrAfter);

            var invalidInternExeption =
                new InvalidInternException();

            invalidInternExeption.AddData(
                key: nameof(Intern.UpdatedDate),
                values: "Date is not recent");

            var expectedInternValidException =
                new InternValidationException(invalidInternExeption);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(invalidIntern.Id))
                    .ReturnsAsync(invalidIntern);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidException);

           this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectInternByIdAsync(randomIntern.Id),
                   Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfInternDoesntExistAndLogItAsync()
        {
            DateTimeOffset randomDateTimeOffset = GetRandomDateTimeOffset();
            Intern nonExistentIntern = CreateRandomIntern();
            nonExistentIntern.UpdatedDate = randomDateTimeOffset;
            Intern noIntern = null;
            var notFoundInternException = new InternNotFoundException(nonExistentIntern.Id);

            var expectedInternValidationException =
                new InternValidationException(notFoundInternException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(nonExistentIntern.Id))
                    .ReturnsAsync(noIntern);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTimeOffset);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(nonExistentIntern);

            // then
            await Assert.ThrowsAsync<InternValidationException>(
               modifyInternTask.AsTask);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(nonExistentIntern.Id),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageInternAuditInformationNotSameAsInputInternAuditInformationAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            DateTimeOffset randomDate = GetRandomDateTime();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            Intern randomIntern = CreateRandomIntern(dates: randomDate);
            Intern invalidIntern = randomIntern;
            Intern storageIntern = randomIntern.DeepClone();
            invalidIntern.CreatedDate = storageIntern.CreatedDate.AddDays(randomNumber);
            invalidIntern.UpdatedDate = storageIntern.UpdatedDate;
            invalidIntern.CreatedBy = invalidCreatedBy;
            Guid internId = invalidIntern.Id;

            var invalidInternException = new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.CreatedDate),
                values: $"Date is not the same as {nameof(Intern.CreatedDate)}");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: $"Date is the same as {nameof(Intern.UpdatedDate)}");

            invalidInternException.AddData(
                key: nameof(Intern.CreatedBy),
                values: $"Id is not the same as {nameof(Intern.CreatedBy)}");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(internId))
                .ReturnsAsync(storageIntern);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDate);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            // then
            InternValidationException actualInternValidationException =
            await Assert.ThrowsAsync<InternValidationException>(() =>
                modifyInternTask.AsTask());

            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(invalidIntern.Id),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
