// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Tests.Acceptance.Brokers;
using Microsoft.Data.SqlClient;
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

        private async Task<Intern> PostRandomInternAsync()
        {
            Intern randomIntern = CreateRandomIntern();
            await this.internTrackApiBroker.PostInternAsync(randomIntern);

            return randomIntern;
        }

        private async Task<List<Intern>> CreateRandomPostedInternsAsync()
        {
            int randomNumber = GetRandomNumber();
            var randomInterns = new List<Intern>();

            for (int i = 0; i < randomNumber; i++)
            {
                randomInterns.Add(await PostRandomInternAsync());
            }

            return randomInterns;
        }

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
