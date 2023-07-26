// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task ShouldThrowDependencyExceptionOnRemoveByIdIfSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedInternStorageException =
                new FailedInternStorageException(sqlException);

            var expectedInterndependencyException =
                new InternDependencyException(failedInternStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Intern> removeInternAsync =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    removeInternAsync.AsTask);


            //then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInterndependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                    expectedInterndependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowdependencyExceptionOnRemoveByIdIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedInternStorageException =
                new FailedInternStorageException(databaseUpdateException);

            var expectedInternDependencyValidationException =
                new InternDependencyException(failedInternStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when 
            ValueTask<Intern> removeInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    removeInternTask.AsTask);

            //then
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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveByIdIfDbUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();

            var databaseUpdateConcurrencyException =
                new DbUpdateConcurrencyException();

            var lockedInternException =
                new LockedInternException(databaseUpdateConcurrencyException);

            var expectedInternDependencyException = 
                new InternDependencyException(lockedInternException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Intern> removeInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            // then
            await Assert.ThrowsAsync<InternDependencyException>(() =>
                removeInternTask.AsTask());

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
        public async Task ShouldThrowServiceExceptionOnRemoveByIdWhenExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(serviceException);

            var expectedInternServiceException =
                new InternServiceException(failedInternServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Intern> removeInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            // then
            await Assert.ThrowsAsync<InternServiceException>(() => 
                removeInternTask.AsTask());

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