// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternValidationException : Xeption
    {
        public InternValidationException(Xeption innerException)
            : base("Intern validation error occurred. Please, try again.",
                       innerException)
        { }

        public InternValidationException(string message, Xeption innerException) 
            : base(message, innerException) 
        { }  
    }
}