// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace InternTrack.Core.Api.Models.Interns.Exceptions
{
    public class InternNotFoundException : Xeption
    {
        public InternNotFoundException(Guid internId)
            : base(message: $"Couldn't find intern with id: {internId}.") 
        { }
    }
}
