// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

 users/ADILATIC/brokers-intern-select-by-id
using System;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
 main

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Intern> Interns { get; set; }

users/ADILATIC/brokers-intern-select-by-id
        public async ValueTask<Intern> SelectInternByIdAsync(Guid internId)
        {
            using var broker =
                   new StorageBroker(this.configuration);

            return await broker.Interns.FindAsync(internId);
        }
    }
}
        public async ValueTask<Intern> DeleteInternsAsync(Intern intern)
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
 main
