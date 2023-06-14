using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using InternTrack.Core.Api.Models.Interns;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldThrowValdationExceptionOnRetrieveByIdIfIdIsInValidAndLogItAsync()
        {
            // given
            Guid randomInternId = default;
            Guid inputInternId = randomInternId;

            var invalidInternException = new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "id is required");

            var expectedInternValidationException =
            new InternValidationException(invalidInternException);

            // when
            ValueTask<Intern> retrieveInternByIdTask =
                this.internService.RetrieveInternByIdAsync(inputInternId);

            // then
            await Assert.ThrowsAsync<InternValidationException>(() =>
                retrieveInternByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionsAs(
                        expectedInternValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
