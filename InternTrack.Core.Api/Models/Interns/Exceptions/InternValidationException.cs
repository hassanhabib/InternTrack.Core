using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternValidationException : Xeption
    {
        public InternValidationException(Xeption innerException)
            : base("Intern validation error occured. Please, try again.",
                       innerException)
        { }
    }
}
