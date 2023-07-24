// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task ShouldThrowDependencyExceptionOnRemoveIfSqlExceptionOccursAndLogItAsync()
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
            ValueTask<Intern> deleteInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            //then
            await Assert.ThrowsAsync<InternDependencyException>(() =>
                deleteInternTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                    expectedInterndependencyException))),
                        Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowdependencyExceptionOnRemoveIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            Guid someInternId = Guid.NewGuid();
            var databaseUpdateException = new DbUpdateException();

            var failedInternStorageException =
                new FailedInternStorageException(databaseUpdateException);

            var expectedInternDependencyException =
                new InternDependencyException(failedInternStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateException);

            // when 
            ValueTask<Intern> deleteInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            //then
            await Assert.ThrowsAsync<InternDependencyException>(() =>
                deleteInternTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternDependencyException))),
                        Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
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
            ValueTask<Intern> deleteInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            // then
            await Assert.ThrowsAsync<InternDependencyException>(() =>
                deleteInternTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs
                    (expectedInternDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveWhenExceptionOccursAndLogItAsync()
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
            ValueTask<Intern> deleteInternTask =
                this.internService.RemoveInternByIdAsync(someInternId);

            // then
            await Assert.ThrowsAsync<InternServiceException>(() => 
                deleteInternTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Once());

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