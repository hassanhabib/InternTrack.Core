// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRemoveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidInternId = Guid.Empty;

            var invalidInternException =
                new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            // when
            ValueTask<Intern> removeInternByIdTask =
                this.internService.RemoveInternByIdAsync(invalidInternId);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(removeInternByIdTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(expectedInternValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                    expectedInternValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteInternAsync(It.IsAny<Intern>()), Times.Never);
            
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
