using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class NullInternException : Xeption
    {
        public NullInternException()
            : base(message: "Intern is null.")
        { }
    }
}
