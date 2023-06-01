// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
     
        public DbSet<Intern> Interns { get; set; }

        public async ValueTask<Intern> InsertInternAsync(Intern intern)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<Intern> internEntityEntry =
                await broker.AddAsync(intern);

            await broker.SaveChangesAsync();

            return internEntityEntry.Entity;
        }

        public IQueryable<Intern> SelectAllInternsAsync()
        {
            using var broker =
               new StorageBroker(this.configuration);

            return broker.Interns;
        }

        public async ValueTask<Intern> SelectInternByIdAsync(Guid internId)
        {
            using var broker =
                   new StorageBroker(this.configuration);

            return await broker.Interns.FindAsync(internId);
        }

        public async ValueTask<Intern> DeleteInternAsync(Intern intern)
        {
            using var broker =
                 new StorageBroker(this.configuration);

            EntityEntry<Intern> internEntityEntry =
                broker.Interns.Remove(intern);

            await broker.SaveChangesAsync();

            return internEntityEntry.Entity;
        }
    }
}