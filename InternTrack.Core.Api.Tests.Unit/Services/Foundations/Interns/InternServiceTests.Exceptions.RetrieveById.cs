// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveIfExceptionOccursAndLogItAsync()
        {
            // given
            var someInternId = Guid.NewGuid();
            var inputInternId = someInternId;
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(serviceException);

            var expectedInternServiceException =
                new InternServiceException(failedInternServiceException);

            this.storageBrokerMock.Setup(broker =>
            broker.SelectInternByIdAsync(inputInternId))
                .ThrowsAsync(serviceException);

            // when
            ValueTask<Intern> retrieveInternByIdTask =
                this.internService.RetrieveInternByIdAsync(someInternId);

            // then
            await Assert.ThrowsAsync<InternServiceException>(() =>
                retrieveInternByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternServiceException))),
                      Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}