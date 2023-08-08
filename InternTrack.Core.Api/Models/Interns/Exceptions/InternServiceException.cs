// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternServiceException : Xeption
    {
        public InternServiceException(Xeption innerException)
            : base(message: "Intern service error occurred, contact support", innerException)
        { }

        public InternServiceException(string message,Exception innerException) 
            : base(message, innerException) 
        { }
    }
}
