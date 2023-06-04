﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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
        public void ShouldRetrieveAllTeachers()
        {
            // given
            IQueryable<Intern> randomInterns = CreateRandomInterns();
            IQueryable<Intern> storageInterns = randomInterns;
            IQueryable<Intern> expectedInterns = storageInterns;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllInternsAsync())
                    .Returns(storageInterns);

            // when
            IQueryable<Intern> actualInterns =
                this.internService.RetrieveAllInternAsync();

            // then
            actualInterns.Should().BeEquivalentTo(expectedInterns);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTimeOffset(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllInternsAsync(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
