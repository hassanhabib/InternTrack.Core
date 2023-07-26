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
        public async void ShouldThrowValidationExceptionOnRemoveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidInternId = Guid.Empty;

            var invalidInternException =
                new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

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
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfInternIsNotFoundAndLogitAsync()
        {
            // given
            Guid randomInternId = Guid.NewGuid();
            Guid inputInternId = randomInternId;
            Intern noIntern = null;

            var notFoundInternException =
                new NotFoundInternException(inputInternId);

            var expectedInternValidationException =
                new InternValidationException(notFoundInternException);

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
