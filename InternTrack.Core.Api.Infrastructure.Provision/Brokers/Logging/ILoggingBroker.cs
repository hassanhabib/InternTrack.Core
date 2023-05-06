// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Logging
{
    public interface ILoggingBroker
    {
        void LogActivity(string message);
    }
}
