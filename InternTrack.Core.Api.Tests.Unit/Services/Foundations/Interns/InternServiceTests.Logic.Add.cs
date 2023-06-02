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
using Force.DeepCloner;
using InternTrack.Core.Api.Models.Interns;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldAddInternAsync()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Models.Interns.Intern randomIntern = CreateRandomIntern();
            Models.Interns.Intern inputIntern = randomIntern;
            Models.Interns.Intern storageIntern = inputIntern;
            Models.Interns.Intern expectedIntern = storageIntern.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertInternAsync(inputIntern))
                    .ReturnsAsync(storageIntern);

            //when

            Models.Interns.Intern actualIntern =
                await this.internService.CreateInternAsync(inputIntern);

            //then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertInternAsync(inputIntern),
                    Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
