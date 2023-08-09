// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InvalidInternException : Xeption
    {
        public InvalidInternException()
            : base(message: "Invalid Intern. Please correct the errors and try again")
        { }

        public InvalidInternException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
