// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        private async Task ShouldThrowDependencyExceptionOnRemoveByIdIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedInternStorageException =
                new FailedInternStorageException(
                    message: "Failed Intern storage error occurred, contact support.",
                        sqlException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        failedInternStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Intern> removeInternAsync =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    removeInternAsync.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

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
        private async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedInternException =
                new LockedInternException(
                    message: "Locked Intern record exception, please try again later.",
                        databaseUpdateConcurrencyException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        lockedInternException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Intern> removeInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    removeInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException );

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs
                    (expectedInternDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowServiceExceptionOnRemoveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(
                    message: "Failed Intern service occurred, please contact support",
                        serviceException);

            var expectedInternServiceException =
                new InternServiceException(
                    message: "Intern service error occurred, contact support",
                        failedInternServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Intern> removeInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternServiceException actualInternServiceException =
                await Assert.ThrowsAsync<InternServiceException>(
                    removeInternTask.AsTask);

            // then
            actualInternServiceException.Should().BeEquivalentTo(
                expectedInternServiceException);

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

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedInternStorageException =
                new FailedInternStorageException(
                    message: "Failed Intern storage error occurred, contact support.",
                        databaseUpdateException);

            var expectedInternDependencyValidationException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        failedInternStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when 
            ValueTask<Intern> removeInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    removeInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}