// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InvalidInternReferenceException : Xeption
    {
        public InvalidInternReferenceException(Exception innerException)
            : base(message: "Invalid intern reference error occurred.", innerException)
        { }
    }
}
