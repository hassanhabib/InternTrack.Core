using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternDependencyValidationException : Xeption
    {
        public InternDependencyValidationException(Xeption innerException)
            : base(message: "Intern dependency validation occurred, please try again.", innerException)
        { }
    }
}
