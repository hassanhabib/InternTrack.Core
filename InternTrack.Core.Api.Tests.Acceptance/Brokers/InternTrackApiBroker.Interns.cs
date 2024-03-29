﻿// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Tests.Acceptance.Brokers
{
    public partial class InternTrackApiBroker
    {
        private const string InternRelativeString = "api/intern";

        public async ValueTask<Intern> PostInternAsync(Intern intern) =>
            await this.apiFactoryClient.PostContentAsync(InternRelativeString, intern);

        public async ValueTask<Intern> GetInternByIdAsync(Guid internId) =>
            await this.apiFactoryClient.GetContentAsync<Intern>($"{InternRelativeString}/{internId}");

        public async ValueTask<List<Intern>> GetAllInternsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Intern>>($"{InternRelativeString}/");

        public async ValueTask<Intern> PutInternAsync(Intern intern) =>
            await this.apiFactoryClient.PutContentAsync<Intern>(InternRelativeString, intern);

        public async ValueTask<Intern> DeleteInternByIdAsync(Guid internId) =>
            await this.apiFactoryClient.DeleteContentAsync<Intern>($"{InternRelativeString}/{internId}");
    }
}
