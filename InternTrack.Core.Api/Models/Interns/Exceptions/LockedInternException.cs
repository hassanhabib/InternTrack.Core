// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class LockedInternException : Xeption
    {
        public LockedInternException(Exception innerException)
            : base(message: "Locked intern record exception, please try again later.", innerException)
        { }
    }
}