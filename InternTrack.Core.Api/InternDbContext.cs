// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Models.Interns;
using System.Data.Entity;

namespace InternTrack.Core.Api
{
    public class InternDbContext : DbContext
    {
        public DbSet<Intern> Interns { get; set; }
    }
}
