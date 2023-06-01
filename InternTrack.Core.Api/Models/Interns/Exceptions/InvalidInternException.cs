// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InvalidInternException : Xeption
    {
        public InvalidInternException()
            : base(message: "Invalid intern. Please correct the errors and try again.")
        { }
    }
}
