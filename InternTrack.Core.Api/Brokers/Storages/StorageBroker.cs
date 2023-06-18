// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
<<<<<<< HEAD
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------
=======
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------
>>>>>>> 315ad10c922567363203346ecf320d5e2303de8f

using EFxceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration
                .GetConnectionString(name: "DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}