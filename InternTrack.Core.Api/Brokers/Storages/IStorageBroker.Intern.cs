// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
 users/ADILATIC/brokers-intern-select-by-id
        ValueTask<Intern> SelectInternByIdAsync(Guid internId);

        ValueTask<Intern> DeleteInternAsync(Intern intern); 
 main
    }
}
