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
        private async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
            SqlException sqlException = GetSqlException();

            var failedInternStorageException =
                new FailedInternStorageException(
                    message: "Failed Intern storage error occurred, contact support.",
                        sqlException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        failedInternStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(sqlException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(someIntern.Id),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateInternAsync(someIntern),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();            
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedInternException =
                new LockedInternException(
                    message: "Locked Intern record exception, please try again later.", 
                        databaseUpdateConcurrencyException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        lockedInternException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateConcurrencyException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(someIntern.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        private async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
            var databaseUpdateException = new DbUpdateException();

            var failedInternStorageException =
                new FailedInternStorageException(
                    message: "Failed Intern storage error occurred, contact support.",
                        databaseUpdateException);

            var expectedInternDependencyException =
                new InternDependencyException(
                    message: "Intern dependency error occurred, contact support.",
                        failedInternStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(someIntern.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
                
        [Fact]
        private async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            Intern someIntern = CreateRandomIntern();
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(
                    message: "Failed Intern service occurred, please contact support",
                        serviceException);

            var expectedInternServiceException =
                new InternServiceException(
                    message: "Intern service error occurred, contact support",
                        failedInternServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(someIntern);

            InternServiceException actualInternDependencyException =
                await Assert.ThrowsAsync<InternServiceException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternServiceException);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternServiceException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(someIntern.Id),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
