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
        private delegate IQueryable<Intern> ReturningQueryableInternFunction();

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
            catch (SqlException sqlException)
            {
                var failedInternStorageException =
                    new FailedInternStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedInternStorageException);
            }
            catch (InvalidInternException invalidInternException)
            {
                throw CreateAndLogValidationException(invalidInternException);
            }
            catch (NotFoundInternException notFoundInternException)
            {
                throw CreateAndLogValidationException(notFoundInternException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsInternException =
                    new AlreadyExistsInternException(duplicateKeyException);

                throw CreateAndLogDependencyValidationException(alreadyExistsInternException);
            }
            catch (ForeignKeyConstraintConflictException foreignKeyConstraintConflictException)
            {
                var invalidInternReferenceException =
                    new InvalidInternReferenceException(foreignKeyConstraintConflictException);

                throw CreateAndLogDependencyValidationException(invalidInternReferenceException);
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

        private IQueryable<Intern> TryCatch(ReturningQueryableInternFunction returningInternFunction)
        {
            try
            {
                return returningInternFunction();
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

        private InternServiceException CreateAndLogServiceException(Xeption exception)
        {
            var InternServiceException =
                new InternServiceException(exception);

            this.loggingBroker.LogError(InternServiceException);

            return InternServiceException;
        }

        private InternDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var InternDependencyException =
                new InternDependencyException(exception);

            this.loggingBroker.LogCritical(InternDependencyException);

            return InternDependencyException;
        }

        private InternValidationException CreateAndLogValidationException(Xeption exception)
        {
            var InternValidationException =
                new InternValidationException(exception);

            this.loggingBroker.LogError(InternValidationException);

            return InternValidationException;
        }

        private InternDependencyValidationException CreateAndLogDependencyValidationException(Xeption exception)
        {
            var InternDependencyValidationException =
                new InternDependencyValidationException(exception);

            this.loggingBroker.LogError(InternDependencyValidationException);

            return InternDependencyValidationException;
        }

        private InternDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var InternDependencyException =
                new InternDependencyException(exception);

            this.loggingBroker.LogError(InternDependencyException);

            return InternDependencyException;
        }
    }
}
