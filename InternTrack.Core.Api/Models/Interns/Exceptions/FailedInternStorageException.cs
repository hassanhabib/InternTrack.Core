using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class FailedInternStorageException : Xeption
    {
        public FailedInternStorageException(Exception innerException)
            : base(message: "Failed intern storage error occurred, contact support.", innerException)
        { }
    }
}
