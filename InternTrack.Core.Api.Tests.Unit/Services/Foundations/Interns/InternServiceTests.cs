using System;
using System.Linq.Expressions;
using InternTrack.Core.Api.Brokers.DateTimes;
using InternTrack.Core.Api.Brokers.Loggings;
using InternTrack.Core.Api.Brokers.Storages;
using InternTrack.Core.Api.Services.Foundations.Interns;
using Moq;
using Tynamix.ObjectFiller;
using Xeptions;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IInternService internService;

        public InternServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.internService = new InternService(
                this.storageBrokerMock.Object,
                this.dateTimeBrokerMock.Object,
                this.loggingBrokerMock.Object);
        }

        private static Models.Interns.Intern CreateRandomIntern() =>
            CreateInternFiller().Create();

        private static Expression<Func<Exception,bool>> SameExceptionsAs(Exception expectedException)
        {
            return actualExpection =>
                expectedException.Message == actualExpection.Message
                && expectedException.InnerException.Message == actualExpection.InnerException.Message;
        }

        private static Expression<Func<Exception, bool>> SameValidationExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message
                && (actualException.InnerException as Xeption).DataEquals(expectedException.InnerException.Data);
        }

        private static Filler<Models.Interns.Intern> CreateInternFiller()
        {
            var filler = new Filler<Models.Interns.Intern>();
            Guid createdById = Guid.NewGuid();

            filler.Setup()
                .OnProperty(intern => intern.CreatedDate).Use(GetRandomDateTime)
                .OnProperty(intern => intern.UpdatedDate).IgnoreIt()
                .OnProperty(intern => intern.CreatedBy).Use(createdById)
                .OnProperty(intern => intern.UpdatedBy).IgnoreIt();

            return filler;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();
    }
}
