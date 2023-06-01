// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class NotFoundInternException : Xeption
    {
        public NotFoundInternException(Guid internId)
            : base(message: $"Couldn't find intern with id: {internId}.")
        { }
    }
}
