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
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            var sqlException = GetSqlException();

            var failedInternStorageException =
                new FailedInternStorageException(sqlException);

            var expectedInternDependencyException =
                new InternDependencyException(failedInternStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Intern> retrieveInternByIdTask =
                this.internService.RetrieveInternByIdAsync(someInternId);

            InternDependencyException actualinternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(retrieveInternByIdTask.AsTask);

            // then
            actualinternDependencyException.Should().BeEquivalentTo(expectedInternDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

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
                broker.SelectInternByIdAsync(someInternId))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Intern> retrieveInternByIdTask =
                this.internService.RetrieveInternByIdAsync(someInternId);

            InternServiceException actualInternServiceException =
                await Assert.ThrowsAsync<InternServiceException>(retrieveInternByIdTask.AsTask);

            // then
            actualInternServiceException.Should().BeEquivalentTo(expectedInternServiceException);

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