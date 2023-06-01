using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternDependencyException : Xeption
    {
        public InternDependencyException(Xeption innerException)
            : base(message: "Intern dependency error occurred, contact support.", innerException)
        { }
    }
}
