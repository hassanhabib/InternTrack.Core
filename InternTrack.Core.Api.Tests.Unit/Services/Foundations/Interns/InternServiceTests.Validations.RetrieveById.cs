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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidInternId = Guid.Empty;
            var invalidInternException = new InvalidInternException();

            invalidInternException.AddData(
               key: nameof(Intern.Id),
               values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            // when
            ValueTask<Intern> retrieveInternByIdTask =
                this.internService.RetrieveInternByIdAsync(invalidInternId);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(retrieveInternByIdTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(expectedInternValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfInternNotFoundAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            Intern noIntern = null;

            var notFoundInternValidationException =
                new NotFoundInternException(someInternId);

            var expectedInternValidationException =
                new InternValidationException(notFoundInternValidationException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(noIntern);

            // when
            ValueTask<Intern> retrieveByIdInternTask =
                this.internService.RetrieveInternByIdAsync(someInternId);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(retrieveByIdInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(expectedInternValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}