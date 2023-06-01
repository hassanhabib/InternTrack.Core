// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Intern> InsertInternAsync(Intern intern);
        IQueryable<Intern> SelectAllInternsAsync();
        ValueTask<Intern> DeleteInternAsync(Intern intern);
    }
}