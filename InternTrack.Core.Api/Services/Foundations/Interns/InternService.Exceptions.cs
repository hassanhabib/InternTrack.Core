﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Xeptions;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public partial class InternService
    {
        private delegate ValueTask<Intern> ReturningInternFunction();
        private delegate IQueryable<Intern> ReturningInternsFunction();

        private async ValueTask<Intern> TryCatch(ReturningInternFunction returningInternFunction)
        {
            try
            {
                return await returningInternFunction();
            }
            catch (NullInternException nullInternException)
            {
                throw CreateAndLogValidationException(nullInternException);
            }
            catch (InvalidInternException invalidInternException)
            {
                throw CreateAndLogValidationException(invalidInternException);
            }
            catch (NotFoundInternException nullInternException)
            {
                throw CreateAndLogValidationException(nullInternException);
            }
            catch (SqlException sqlException)
            {
                var failedInternStorageException =
                    new FailedInternStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedInternStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsInternException =
                    new AlreadyExistsInternException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsInternException);
            }
<<<<<<< HEAD
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedInternException = new LockedInternException(dbUpdateConcurrencyException);
=======
            catch (DbUpdateConcurrencyException dbUpdateConcurrrencyException)
            {
                var lockedInternException = new LockedInternException(dbUpdateConcurrrencyException);
>>>>>>> 358410566640ee894adb123da3beb059fb9c61b9

                throw CreateAndLogDependencyException(lockedInternException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedStorageInternException =
                    new FailedInternStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedStorageInternException);
            }
            catch (Exception exception)
            {
                var failedInternServiceException =
                    new FailedInternServiceException(exception);

                throw CreateAndLogServiceException(failedInternServiceException);
            }
        }

        private IQueryable<Intern> TryCatch(ReturningInternsFunction returningInternsFunction)
        {
            try
            {
                return returningInternsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedInternStorageException =
                    new FailedInternStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedInternStorageException);
            }
            catch (Exception exception)
            {
                var failedInternServiceException =
                    new FailedInternServiceException(exception);

                throw CreateAndLogServiceException(failedInternServiceException);
            }
        }

        private InternValidationException CreateAndLogValidationException(
            Xeption exception)
        {
            var internValidationException =
                new InternValidationException(exception);

            this.loggingBroker.LogError(internValidationException);

            return internValidationException;
        }

        private InternDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var internDependencyException =
                new InternDependencyException(exception);

            this.loggingBroker.LogCritical(internDependencyException);

            return internDependencyException;
        }

        private InternDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var internDependencyValidationException =
                new InternDependencyValidationException(exception);

            this.loggingBroker.LogError(internDependencyValidationException);

            return internDependencyValidationException;
        }

        private InternDependencyException CreateAndLogDependencyException(
            Xeption exception)
        {
            var InternDependencyException =
                new InternDependencyException(exception);

            this.loggingBroker.LogError(InternDependencyException);

            return InternDependencyException;
        }

        private InternServiceException CreateAndLogServiceException(
            Xeption exception)
        {
            var InternServiceException =
                new InternServiceException(exception);

            this.loggingBroker.LogError(InternServiceException);

            return InternServiceException;
        }
    }
}
