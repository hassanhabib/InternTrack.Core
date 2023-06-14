// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using InternTrack.Core.Api.Models.Interns.Exceptions;
using InternTrack.Core.Api.Models.Interns;
using Microsoft.Data.SqlClient;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDepdnencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset datetime = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern(datetime);
            randomIntern.CreatedDate = datetime;
            randomIntern.UpdatedDate = randomIntern.CreatedDate.AddMinutes(GetRandomNumber());

            SqlException sqlException = GetSqlException();

            var failedInternStorageException =
                new FailedInternStorageException(sqlException);

            var expectedInternDependencyExcetpion =
                new InternDependencyException(failedInternStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomIntern.UpdatedDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(randomIntern.Id))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(randomIntern);

            InternDependencyException actualInternDependecyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependecyException.Should().BeEquivalentTo(
                expectedInternDependencyExcetpion);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(randomIntern.Id),
                    Times.Once());

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
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset datetime = GetRandomDateTime();
            Intern randomIntern = CreateRandomIntern(datetime);
            randomIntern.CreatedDate = datetime;
            randomIntern.UpdatedDate = randomIntern.CreatedDate.AddMinutes(GetRandomNumber());

            var databaseUpdateException = new DbUpdateException();

            var failedInternStorageException =
                new FailedInternStorageException(databaseUpdateException);

            var expectedInternDependencyException =
                new InternDependencyException(failedInternStorageException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTimeOffset())
                    .Returns(randomIntern.UpdatedDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(randomIntern.Id))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(randomIntern);

            InternDependencyException actualInternDependencyException =
                await Assert.ThrowsAsync<InternDependencyException>(
                    modifyInternTask.AsTask);

            // then
            actualInternDependencyException.Should().BeEquivalentTo(
                expectedInternDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(randomIntern.Id),
                    Times.Once());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
