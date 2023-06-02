// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public partial class InternService
    {
        private delegate ValueTask<Intern> ReturningInternFuction();

        private async ValueTask<Intern> TryCatch(ReturningInternFuction returningInternFuction)
        {
            try
            {
                return await returningInternFuction();
            }
            catch(NullInternException nullInternException)
            {
                throw CreateAndLogicValidationException(nullInternException);
            }
            catch(InvalidInternException invalidInternException)
            {
                throw CreateAndLogicValidationException(invalidInternException);
            }
            catch(SqlException sqlException)
            {
                var failedInternStorageException = new FailedInternStorageException(sqlException);
                throw CreateAndLogCriticalDependencyException(failedInternStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsInternException =
                    new AlreadyExistsInternException(duplicateKeyException);

                throw Create
            }
        }

        private InternDependencyValidationException CreateAndLogDependencyValidationException(
            Xeption exception)
        {
            var internDependencyValidationException =
                new InternDependencyValidationException(exception);

            this.loggingBroker.LogError(internDependencyValidationException);

            return internDependencyValidationException;
        }

        private InternDependencyException CreateAndLogCriticalDependencyException(
            Xeption exception)
        {
            var internDepencyException = 
                new InternDependencyException(exception);
            this.loggingBroker.LogCritical(internDepencyException);

            return internDepencyException;
        }

        private InternValidationException CreateAndLogicValidationException(
            Xeption exception)
        {
            var internValidationException =
                new InternValidationException(exception);

            this.loggingBroker.LogError(internValidationException);

            return internValidationException;
        }
    }
}
