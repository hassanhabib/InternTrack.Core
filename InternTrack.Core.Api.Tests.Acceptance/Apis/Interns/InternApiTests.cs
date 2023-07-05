// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace InternTrack.Core.Api.Tests.Acceptance.Apis.Interns
{
    [Collection(nameof(ApiTestCollection))]
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
            DateTimeOffset now = DateTimeOffset.UtcNow;
            Guid internId = Guid.NewGuid();
            var filler = new Filler<Intern>();

            filler.Setup()
                .OnProperty(intern => intern.CreatedBy).Use(internId)
                .OnProperty(intern => intern.UpdatedBy).Use(internId)
                .OnProperty(intern => intern.CreatedDate).Use(now)
                .OnProperty(intern => intern.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime);

            return filler;
        }
    }
}
