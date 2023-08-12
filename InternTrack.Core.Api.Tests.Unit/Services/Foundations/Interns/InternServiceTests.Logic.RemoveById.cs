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
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        private async Task ShouldRemoveInternByIdAsync()
        {
            // given
            Guid randomId = Guid.NewGuid();
            Guid inputInternId = randomId;
            Intern randomIntern = CreateRandomIntern();
            Intern storageIntern = randomIntern;
            Intern expectedInputIntern = storageIntern;
            Intern removedIntern = expectedInputIntern;
            Intern expectedIntern = removedIntern.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternByIdAsync(inputInternId))
                    .ReturnsAsync(storageIntern);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteInternAsync(expectedInputIntern))
                    .ReturnsAsync(removedIntern);
            
            // when
            Intern actualIntern =
                await this.internService.RemoveInternByIdAsync(inputInternId);

            // then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternByIdAsync(inputInternId),
                    Times.Once);
            
            this.storageBrokerMock.Verify(broker =>
                broker.DeleteInternAsync(storageIntern),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}