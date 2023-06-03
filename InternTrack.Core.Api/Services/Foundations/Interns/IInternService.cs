using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public interface IInternService
    {
        ValueTask<Intern> AddInternAsync(Intern intern);
    }
}
