using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public partial class InternService
    {
        private static void ValidateIntern(Intern intern)
        {
            ValidateInternIsNotNull(intern);
        }

        private static void ValidateInternIsNotNull(Intern intern)
        {
            if(intern is null)
            {
                throw new NullInternException();
            }
        }

        private static dynamic isInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic isInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic isInvalid(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void Validate(params (dynamic Rule, string Pramaeter)[] validations)
        {
            var invalidInternException = new InvalidInternException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidInternException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidInternException.ThrowIfContainsErrors();
        }
    }
}
