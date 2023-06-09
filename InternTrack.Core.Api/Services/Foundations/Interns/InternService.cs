// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Linq;
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
<<<<<<< HEAD
        TryCatch(async () =>
        {
            ValidateInternOnAdd(intern);

            return await this.storageBroker.InsertInternAsync(intern);
        });                    

        public IQueryable<Intern> RetrieveAllInternsAsync() =>
            TryCatch(() => this.storageBroker.SelectAllInternsAsync());
=======
            TryCatch(async () =>
            {
                ValidateInternOnAdd(intern);

                return await this.storageBroker.InsertInternAsync(intern);
            });
>>>>>>> 2f01c36c3700d66a50c3c4ca5e60201cd403a40c
    }
}
