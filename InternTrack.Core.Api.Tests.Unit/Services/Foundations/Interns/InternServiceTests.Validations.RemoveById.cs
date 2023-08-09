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
        private async void ShouldThrowValidationExceptionOnRemoveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidInternId = Guid.Empty;
            var innerException = new Exception();

            var invalidInternException =
                new InvalidInternException(
                    message: "Invalid intern. Please correct the errors and try again",
                        innerException);

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        invalidInternException);

            // when
            ValueTask<Intern> removeInternByIdTask =
                this.internService.RemoveInternByIdAsync(invalidInternId);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(removeInternByIdTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(expectedInternValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteInternAsync(It.IsAny<Intern>()),
                    Times.Never);
            
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowNotFoundExceptionOnRemoveIfInternIsNotFoundAndLogitAsync()
        {
            // given
            Guid randomInternId = Guid.NewGuid();
            Guid inputInternId = randomInternId;
            Intern noIntern = null;
            var innerException = new Exception();

            var notFoundInternException =
                new NotFoundInternException(
                     message: $"Intern with id: {inputInternId} not found, please correct and try again.",
                        innerException);

            var invalidInternException =
                new InvalidInternException(
                    message: "Invalid intern. Please correct the errors and try again",
                        innerException);

            var expectedInternValidationException =
                new InternValidationException(
                    message: "Intern validation error occurred. Please, try again.",
                        notFoundInternException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noIntern);

            // when
            ValueTask<Intern> removeInternByIdTask =
                this.internService.RemoveInternByIdAsync(inputInternId);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    removeInternByIdTask.AsTask);

            // then
            actualInternValidationException.Should()
                .BeEquivalentTo(expectedInternValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);
                    
            this.storageBrokerMock.Verify(broker =>
                broker.DeleteInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
