// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Sockets;
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

        public InternService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Intern> AddInternAsync(Intern intern) =>
        TryCatch(async () =>
        {
            ValidateInternOnAdd(intern);

            return await this.storageBroker.InsertInternAsync(intern);
        });

        public ValueTask<Intern> RetrieveInternByIdAsync(Guid internId) =>
        TryCatch(async () =>
        {
            ValidateInternId(internId);

            Intern maybeIntern =
                await this.storageBroker.SelectInternByIdAsync(internId);

            ValidateStorageIntern(maybeIntern, internId);

            return maybeIntern;
        });

        public IQueryable<Intern> RetrieveAllInternsAsync() =>
            TryCatch(() => this.storageBroker.SelectAllInternsAsync());

        public ValueTask<Intern> RemoveInternByIdAsync(Guid internId) =>
            TryCatch(async () =>
            {
                ValidateInternId(internId);
                
                Intern maybeIntern = 
                    await this.storageBroker.SelectInternByIdAsync(internId);

                return await this.storageBroker.DeleteInternAsync(maybeIntern);
            });
    }
}