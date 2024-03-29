﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
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
        private async Task ShouldThrowValidationExceptionOnModifyIfInternIsNullAndLogItAsync()
        {
            // given
            Intern nullIntern = null;
            var innerException = new Exception();

            var nullInternException =
                new NullInternException(message: "Intern is null.", innerException: innerException);

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: nullInternException);

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
                broker.UpdateInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();            
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        private async Task ShouldThrowValidationExceptionOnModifyIfInternIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var innerException = new Exception();

            var invalidIntern = new Intern
            {
                FirstName = invalidText
            };

            var invalidInternException = new InvalidInternException(
                message: "Invalid Intern. Please correct the errors and try again",
                    innerException: innerException);

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.FirstName),
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
                values: new String[] { "Date is required", 
                    $"Date is the same as {nameof(Intern.CreatedDate)}"});

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
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: invalidInternException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern(dates: randomDateTime);
            Intern invalidIntern = randomIntern;
            var innerException = new Exception();

            var invalidInternException =
                new InvalidInternException(
                    message: "Invalid Intern. Please correct the errors and try again",
                        innerException: innerException);

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: $"Date is the same as {nameof(Intern.CreatedDate)}");

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: invalidInternException);

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
                expectedInternValidationException);

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

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(MinutesBeforeOrAfter))]
        private async Task ShouldThrowValidationExceptionOnModifyIfUpdatedDateIsNotRecentAndLogItAsync(
            int minutesBeforeOrAfter)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern(dates: randomDateTime);
            Intern invalidIntern = randomIntern;
            var innerException = new Exception();

            invalidIntern.UpdatedDate = 
                invalidIntern.UpdatedDate
                    .AddMinutes(minutesBeforeOrAfter);

            var invalidInternException =
                new InvalidInternException(
                    message: "Invalid Intern. Please correct the errors and try again",
                        innerException: innerException);

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: "Date is not recent");

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: invalidInternException);

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
                expectedInternValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                 broker.GetCurrentDateTimeOffset(),
                     Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectInternByIdAsync(randomIntern.Id),
                   Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfInternDoesNotExistAndLogItAsync()
        {
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();            
            Intern noIntern = null;
            var innerException = new Exception();

            Intern nonExistentIntern = 
                CreateRandomModifyIntern(dates: randomDateTime);

            var notFoundInternException = 
                new NotFoundInternException(
                     message: $"Intern with id: {nonExistentIntern.Id} not found, please correct and try again.",
                        innerException: innerException);

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: notFoundInternException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(nonExistentIntern.Id))
                    .ReturnsAsync(noIntern);
                                                                        
            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(nonExistentIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);
                        
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(nonExistentIntern.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);
                        
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateIsNotTheSameAsCreatedDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNegativeNumber();
            Guid invalidCreatedByGuid = Guid.NewGuid();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Intern randomIntern = CreateRandomModifyIntern(randomDateTime);
            Intern invalidIntern = randomIntern.DeepClone();
            Intern storageIntern = invalidIntern.DeepClone();
            storageIntern.CreatedDate = storageIntern.CreatedDate.AddMinutes(randomNumber);
            storageIntern.UpdatedDate = storageIntern.UpdatedDate.AddMinutes(randomNumber);
            Guid internId = invalidIntern.Id;
            var innerException = new Exception();

            var invalidInternException = 
                new InvalidInternException(
                    message: "Invalid Intern. Please correct the errors and try again",
                        innerException: innerException);

            invalidInternException.AddData(
                key: nameof(Intern.CreatedDate),
                values: $"Date is not the same as {nameof(Intern.CreatedDate)}");

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: invalidInternException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(internId))
                    .ReturnsAsync(storageIntern);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(() =>
                    modifyInternTask.AsTask());

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                expectedInternValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(invalidIntern.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);
                        
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Intern randomIntern = CreateRandomModifyIntern(randomDateTime);
            Intern invalidIntern = randomIntern;
            Intern storageIntern = invalidIntern.DeepClone();
            invalidIntern.UpdatedDate = storageIntern.UpdatedDate;
            Guid internId = invalidIntern.Id;
            var innerException = new Exception();

            var invalidInternException =
                new InvalidInternException(
                    message: "Invalid Intern. Please correct the errors and try again",
                        innerException: innerException);

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                values: $"Date is the same as {nameof(Intern.UpdatedDate)}");

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        innerException: invalidInternException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(internId))
                    .ReturnsAsync(storageIntern);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomDateTime);

            //when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    modifyInternTask.AsTask);

            //then
            actualInternValidationException.Should()
                .BeEquivalentTo(
                    expectedInternValidationException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(internId),
                    Times.Once);
                        
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
