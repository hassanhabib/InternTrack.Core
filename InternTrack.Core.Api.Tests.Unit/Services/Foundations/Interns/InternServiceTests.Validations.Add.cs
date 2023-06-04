// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfInternIsNullAndLogItAsync()
        {
            // given
            Intern nullIntern = null;

            var nullInternException =
                new NullInternException();

            var expectedInternValidationException =
                new InternValidationException(nullInternException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(nullIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    addInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnAddIfInternIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidIntern = new Intern
            {
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText,
                Email = invalidText,
                PhoneNumber = invalidText,
                Status = invalidText,

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
                key: nameof(Intern.CreatedDate),
                values: "Date is required");

            invalidInternException.AddData(
                key: nameof(Intern.CreatedBy),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: "Date is required");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedBy),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.JoinDate),
                values: "Date is required");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    addInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfCreateAndUpdateDatesIsNotSameAndLogitAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Intern randomIntern = CreateRandomIntern(randomDateTime);
            Intern invalidIntern = randomIntern;
            invalidIntern.UpdatedBy = randomIntern.CreatedBy;
            invalidIntern.UpdatedDate = GetRandomDateTime();

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            var invalidInternException =
                new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: $"Date is not the same as {nameof(Intern.CreatedDate)}");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    addInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                        expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        public async Task ShouldThrowValidationExceptionOnAddIfCreatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime =
                GetRandomDateTimeOffset();

            DateTimeOffset invalidDateTime =
                randomDateTime.AddMinutes(minutesBeforeOrAfter);

            Intern randomIntern = CreateRandomIntern(invalidDateTime);
            Intern invalidIntern = randomIntern;

            var invalidInternExeption =
                new InvalidInternException();

            invalidInternExeption.AddData(
                key: nameof(Intern.CreatedDate),
                values: "Date is not recent");

            var expectedInternValidException =
                new InternValidationException(invalidInternExeption);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(randomIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    addInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                        expectedInternValidException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
