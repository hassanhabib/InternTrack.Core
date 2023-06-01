using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternServiceException : Xeption
    {
        public InternServiceException(Xeption innerException)
            : base(message: "Intern service error occurred, please contact support.", innerException)
        {
        }
    }
}
