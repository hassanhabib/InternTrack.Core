// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using InternTrack.Core.Api.Brokers.DateTimes;
using InternTrack.Core.Api.Brokers.Loggings;
using InternTrack.Core.Api.Brokers.Storages;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public partial class InternService : IInternService
    {
        public readonly IStorageBroker storageBroker;
        public readonly IDateTimeBroker dateTimeBroker;
        public readonly ILoggingBroker loggingBroker;

        public InternService(IStorageBroker storageBroker, IDateTimeBroker dateTimeBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Intern> AddInternAsync(Intern intern)
        {
            return await this.storageBroker.InsertInternAsync(intern);
        }
        public ValueTask<Intern> ModifyInternAsync(Intern intern) =>
            TryCatch(async () =>
            {
                ValidateInternOnModify(intern);

                Intern maybeIntern =
                    await this.storageBroker.SelectInternByIdAsync(intern.Id);

                return await this.storageBroker.UpdateInternAsync(intern);
            });
    }
}
