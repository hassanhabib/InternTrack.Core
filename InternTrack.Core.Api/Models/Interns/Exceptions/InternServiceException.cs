﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternServiceException : Xeption
    {
        public InternServiceException(Xeption innterException)
            : base(message: "Intern service error occured, contact support", innterException)
        { }
    }
}
