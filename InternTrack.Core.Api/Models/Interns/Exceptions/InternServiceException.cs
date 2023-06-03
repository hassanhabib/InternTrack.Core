using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternServiceException : Xeption
    {
        public InternServiceException(Exception innterException)
            : base(message: "Intern service error occured, contact support", innterException)
        { }
    }
}
