// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class FailedInternStorageException : Xeption
    {
        public FailedInternStorageException(Exception innerException)
            : base("Failed intern storage error occurred, contact support.", innerException)
        { }

        public FailedInternStorageException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
