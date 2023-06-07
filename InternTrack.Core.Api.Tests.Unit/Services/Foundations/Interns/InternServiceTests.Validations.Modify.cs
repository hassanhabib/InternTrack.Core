using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenInternIsNullAndLogItAsync()
        {
            // given
            Intern nullIntern = null;

            var nullInternException =
                new NullInternException();

            var expectedInternValidationException =
                new InternValidationException(nullInternException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(nullIntern);

            NullInternException actualInternValidationException =
                await Assert.ThrowsAsync<NullInternException>(
                    modifyInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                nullInternException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenInternIsInvalidAndLogItAsync(string invalidText)
        {
            // given
            var invalidIntern = new Intern
            {
                FirstName = invalidText,
                MiddleName = invalidText,
                LastName = invalidText,
                Email = invalidText,
                PhoneNumber = invalidText,
                Status = invalidText
            };

            var invalidInternException = new InvalidInternException();

            invalidInternException.AddData(
                key: nameof(Intern.Id),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.FirstName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.MiddleName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.LastName),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.Email),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.PhoneNumber),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.Status),
                values: "Text is required");

            invalidInternException.AddData(
                key: nameof(Intern.CreatedDate),
                values: "Date is required");

            invalidInternException.AddData(
                key: nameof(Intern.CreatedBy),
                values: "Id is required");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedDate),
                "Date is required",
                $"Date is the same as {nameof(Intern.CreatedDate)}");

            invalidInternException.AddData(
                key: nameof(Intern.UpdatedBy),
                values: "Id is required");

            var expectedInternValidationException =
                new InternValidationException(invalidInternException);

            // when
            ValueTask<Intern> modifyInternTask =
                this.internService.ModifyInternAsync(invalidIntern);

            InternValidationException actualInternValidationException =
                await Assert.ThrowsAsync<InternValidationException>(
                    modifyInternTask.AsTask);

            // then
            actualInternValidationException.Should().BeEquivalentTo(
                invalidInternException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(It.IsAny<Intern>()),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedInternValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
