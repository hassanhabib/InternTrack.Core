using System;
using System.Linq;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Intern> InsertInternAsync(Intern intern);
        IQueryable<Intern> SelectAllIntern();
        ValueTask<Intern> SelectPostByIdAsync(Guid internId);
        ValueTask<Intern> DeleteInternAsync(Intern intern);
        ValueTask<Intern> UpdateInternAsync(Intern intern);
    }
}
