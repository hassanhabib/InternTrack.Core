// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

 users/ADILATIC/brokers-intern-select-all
using System.Linq;
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

 users/ADILATIC/brokers-intern-select-all
        public IQueryable<Intern> SelectAllIntern()
        {
            using var broker =
               new StorageBroker(this.configuration);

            return broker.Interns;
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
