// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Intern> Interns { get; set; }

 users/ADILATIC/brokers-intern-insert
        public async ValueTask<Intern> InsertInternAsync(Intern intern)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Intern> internEntityEntry = await broker.AddAsync(intern);

        public async ValueTask<Intern> DeleteInternsAsync(Intern intern)
        {
            using var broker =
                 new StorageBroker(this.configuration);

            EntityEntry<Intern> internEntityEntry =
                broker.Interns.Remove(intern);

 main
            await broker.SaveChangesAsync();

            return internEntityEntry.Entity;
        }
users/ADILATIC/brokers-intern-insert

 main
    }
}
