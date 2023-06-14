using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;

namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Interns
{
    public partial class InternApiTests
    {
        private readonly InternTrackApiBroker internTrackApiBroker;

        public InternApiTests(InternTrackApiBroker internTrackApiBroker) =>
            this.internTrackApiBroker = internTrackApiBroker;

        private static Intern CreateRandomIntern() =>
            CreateInternFiller().Create();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static Filler<Intern> CreateInternFiller()
        {
            var filler = new Filler<Intern>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(GetRandomDateTime);

            return filler;
        }
    }
}
