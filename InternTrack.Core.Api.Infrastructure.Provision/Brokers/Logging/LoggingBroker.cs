// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;

namespace InternTrack.Core.Api.Infrastructure.Provision.Brokers.Logging
{
    public class LoggingBroker : ILoggingBroker
    {
        private readonly ILogger logger;

        public LoggingBroker(ILogger logger) =>
            this.logger = logger;

        public void LogInformation(string message) =>
            this.logger.LogInformation(message);

        public void LogTrace(String message) =>
            this.logger.LogTrace(message);

        public void LogDebug(string message) =>
            this.logger.LogDebug(message);

        public void LogWarning(string message) =>
            this.logger.LogWarning(message);

        public void LogError(Exception exception) =>
            this.logger.LogError(exception.Message, exception);

        public void LogCritical(Exception exception) =>
            this.logger.LogCritical(exception, exception.Message);
    }
}
