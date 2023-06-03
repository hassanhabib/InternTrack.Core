// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class FailedInternServiceException : Xeption
    {
        public FailedInternServiceException(Exception innerException)
            : base("Failed intern service occured, please contact support", innerException)
        { }
    }
}
