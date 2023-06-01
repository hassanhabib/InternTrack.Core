// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using InternTrack.Core.Api.Models.Interns;
using System.Linq;
using System.Threading.Tasks;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Intern> DeleteInternAsync(Intern intern);
        ValueTask<Intern> InsertInternAsync(Intern intern);
        IQueryable<Intern> SelectAllInternsAsync();
    }
}