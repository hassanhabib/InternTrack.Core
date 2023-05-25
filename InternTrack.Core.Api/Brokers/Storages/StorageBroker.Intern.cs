// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Linq;
using InternTrack.Core.Api.Models.Interns;
using Microsoft.EntityFrameworkCore;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Intern> Interns { get; set; }

        public IQueryable<Intern> SelectAllIntern()
        {
            using var broker =
               new StorageBroker(this.configuration);

            return broker.Interns;
        }
    }
}