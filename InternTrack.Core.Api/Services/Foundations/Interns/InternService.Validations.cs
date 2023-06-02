using System;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public partial class InternService
    {
        private static void ValidateIntern(Intern intern)
        {
            ValidateInternIsNotNull(intern);
        }

        private static void ValidateInternIsNotNull(Intern intern)
        {
            if(intern is null)
            {
                throw new NullInternException();
            }
        }
    }
}
