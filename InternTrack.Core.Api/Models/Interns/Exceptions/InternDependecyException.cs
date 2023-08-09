// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternDependencyException : Xeption
    {
        public InternDependencyException(Xeption innerException)
            : base(message: "Intern dependency error occurred, contact support.", innerException)
        { }

        public InternDependencyException(string message, Xeption innerException)
            : base(message, innerException) 
        { }
    }
}
