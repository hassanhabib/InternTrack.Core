using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using InternTrack.Core.Api.Models.Interns;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveInternById()
        {
            //given
            Intern randomIntern = CreateRandomIntern();
            Guid inputInternId = randomIntern.Id;
            Intern storageIntern = randomIntern;
            Intern expectedIntern = storageIntern;

            this.storageBrokerMock.Setup(broker => broker.SelectInternByIdAsync(inputInternId))
                .ReturnsAsync(storageIntern);
            
            //when
            Intern actualIntern =
                    await this.internService.RetrieveInternByIdAsync(inputInternId);
            
            //then
            actualIntern.Should().BeEquivalentTo(expectedIntern);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternById(inputInternId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}