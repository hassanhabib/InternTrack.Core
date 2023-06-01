// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class AlreadyExistsInternException : Xeption
    {
        public AlreadyExistsInternException(Exception innerException)
            : base(message: "Intern with the same id already exists.", innerException)
        { }
    }
}
