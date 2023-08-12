// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        private void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlErrorOccursAndLogIt()
        {
            // given
            SqlException sqlException = GetSqlException();

            var failedInternStorageException =
                new FailedInternStorageException(
                    message: "Failed Intern storage error occurred, contact support.",
                        innerException: sqlException);

            var expectedInternStorageException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        innerException: failedInternStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllInternsAsync())
                    .Throws(sqlException);

            // when
            Action retrieveAllInternsAction = () =>
                this.internService.RetrieveAllInternsAsync();

            InternDependencyException actualInternDependencyException =
                Assert.Throws<InternDependencyException>(retrieveAllInternsAction);

            // then
            actualInternDependencyException.Should()
                .BeEquivalentTo(expectedInternStorageException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                    expectedInternStorageException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllInternsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private void ShouldThrowServiceExceptionOnRetrieveAllIfServiceErrorOccursAndLogIt()
        {
            // given
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(
                    message: "Failed Intern service occurred, please contact support",
                        innerException: serviceException);

            var expectedInternServiceException =
                new InternServiceException(
                    message: "Intern service error occurred, contact support",
                        innerException: failedInternServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllInternsAsync())
                    .Throws(serviceException);

            // when
            Action retrieveAllInternAction = () =>
                this.internService.RetrieveAllInternsAsync();

            InternServiceException actualInternServiceException =
                Assert.Throws<InternServiceException>(retrieveAllInternAction);

            // then
            actualInternServiceException.Should()
                .BeEquivalentTo(expectedInternServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllInternsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
