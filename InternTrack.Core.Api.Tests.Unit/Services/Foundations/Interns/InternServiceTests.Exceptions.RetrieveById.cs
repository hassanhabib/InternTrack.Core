using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServicesTests
    {
        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogIt()
        {
            // given
            Guid someId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedInternServiceException =
                new FailedInternServiceException(serviceException);

            var expectedInternServiceException =
                new BookServiceException(failedInternServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectInternById(It.IsAny<Guid>()))
                    .Throws(serviceException);

            // when
            Action reretrieveInternByIdAction = () =>
                this.internService.RetrieveInternById(someId);

            // then 
            Assert.Throws<InternServiceException>(reretrieveInternByIdAction);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectInternById(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedInternServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
