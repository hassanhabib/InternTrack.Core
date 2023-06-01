using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class FailedInternServiceException : Xeption
    {
        public FailedInternServiceException(Exception innerException)
            : base(message: "Failed intern service occurred, please contact support.", innerException)
        { }
    }
}
