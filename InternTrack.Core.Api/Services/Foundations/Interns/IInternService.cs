﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public interface IInternService
    {
        ValueTask<Intern> AddInternAsync(Intern intern);
        ValueTask<Intern> RetrieveInternByIdAsync(Guid internId);
        IQueryable<Intern> RetrieveAllInternsAsync();
    }
}