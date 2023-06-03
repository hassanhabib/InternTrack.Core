// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDepdnencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Intern randomIntern = CreateRandomIntern();
            SqlException sqlException = GetSqlException();

            var failedInternStorageException =
                new FailedInternStorageException(sqlException);

            var expectedInternDependencyExcetpion =
                new InternDependencyException(failedInternStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                            broker.GetCurrentDateTimeOffset())
                                .Throws(sqlException);

            // when
            ValueTask<Intern> createIntern =
                this.internService.AddInternAsync(randomIntern);

            InternDependencyException actualInternDependecyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    createIntern.AsTask);

            // then
            actualInternDependecyException.Should().BeEquivalentTo(
                expectedInternDependencyExcetpion);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                        expectedInternDependencyExcetpion))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfInternAlreadyExsitsAndLogItAsync()
        {
            // given
            Intern randomIntern = CreateRandomIntern();
            Intern alreadyExistsIntern = randomIntern;
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
                new DuplicateKeyException(randomMessage);

            var alreadyExistsInternException =
                new AlreadyExistsInternException(duplicateKeyException);

            var expectedInternDependencyValidationExcetption =
                new InternDependencyValidationException(alreadyExistsInternException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(duplicateKeyException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(alreadyExistsIntern);

            InternDependencyValidationException actualInternDependencyValidationException =
                await Assert.ThrowsAsync<InternDependencyValidationException>(
                    addInternTask.AsTask);

            // then
            actualInternDependencyValidationException.Should().BeEquivalentTo(
                expectedInternDependencyValidationExcetption);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                        expectedInternDependencyValidationExcetption))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Intern randomIntern = CreateRandomIntern();

            var databaseUpdateException =
                new DbUpdateException();

            var failedInternStorageException =
                new FailedInternStorageException(databaseUpdateException);

            var expectedInternDependencyException =
                new InternDependencyException(failedInternStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(databaseUpdateException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(randomIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    addInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                        expectedInternDependencyException))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            // given
            Intern randomIntern = CreateRandomIntern();
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(serviceException);

            var expectedInternServiceExcetpion =
                new InternServiceException(failedInternServiceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Throws(serviceException);

            // when
            ValueTask<Intern> addInternTask =
                this.internService.AddInternAsync(randomIntern);

            InternServiceException actualInternServiceException =
                await Assert.ThrowsAsync<InternServiceException>(
                    addInternTask.AsTask);

            // then
            actualInternServiceException.Should().BeEquivalentTo(
                expectedInternServiceExcetpion);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionsAs(
                        expectedInternServiceExcetpion))),
                            Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
