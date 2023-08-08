// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class AlreadyExistsInternException : Xeption
    {
        public AlreadyExistsInternException(Exception innerException)
            : base(message: "Intern with the same id already exists.", innerException)
        { }

        public AlreadyExistsInternException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
