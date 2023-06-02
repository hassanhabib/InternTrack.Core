using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternTrack.Core.Api.Brokers.Loggings;
using InternTrack.Core.Api.Brokers.Storages;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Services.Foundations.Interns;
using Moq;
using Tynamix.ObjectFiller;

namespace InternTrack.Core.Api.Tests.Unit.Services.Foundations.Interns
{
    public partial class InternServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly Mock<IInternService> internService;

        public InternServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.internService = new Mock<IInternService>(
                this.storageBrokerMock,
                this.loggingBrokerMock);
        }

        private static Models.Interns.Intern CreateRandomIntern() =>
            CreateInternFiller().Create();

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
