using InternTrack.Core.Api.Models.Interns;
using System.Threading.Tasks;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public interface IInternService
    {
        ValueTask<Intern> CreateInternAsync(Intern intern);
    }
}
