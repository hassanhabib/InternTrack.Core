// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class NotFoundInternException : Xeption
    {
        public NotFoundInternException(Guid internId)
            : base(message: $"Couldn't find intern id: {internId}.")
        { }

        public NotFoundInternException(string message,Exception innerException)
            : base(message, innerException) 
        { }
    }
}