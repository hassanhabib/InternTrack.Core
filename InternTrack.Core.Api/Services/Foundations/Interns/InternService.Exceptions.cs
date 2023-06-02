// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System.Threading.Tasks;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
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
            catch (NullInternException nullInternException)
            {
                throw CreateAndLogicValidationException(nullInternException);
            }
            catch (InvalidInternException invalidInternException)
            {
                throw CreateAndLogicValidationException(invalidInternException);
            }
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
