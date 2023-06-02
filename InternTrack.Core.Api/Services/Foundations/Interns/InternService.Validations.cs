using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using InternTrack.Core.Api.Models.Interns;
using InternTrack.Core.Api.Models.Interns.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InternTrack.Core.Api.Services.Foundations.Interns
{
    public partial class InternService
    {
        private static void ValidateIntern(Intern intern)
        {
            ValidateInternIsNotNull(intern);

            Validate(
                (Rule: isInvalid(intern.Id), Parameter: nameof(Intern.Id)),
                (Rule: isInvalid(intern.FirstName), Parameter: nameof(Intern.FirstName)),
                (Rule: isInvalid(intern.MiddleName), Parameter: nameof(Intern.MiddleName)),
                (Rule: isInvalid(intern.LastName), Parameter: nameof(Intern.LastName)),
                (Rule: isInvalid(intern.Email), Parameter: nameof(Intern.Email)),
                (Rule: isInvalid(intern.PhoneNumber), Parameter: nameof(Intern.PhoneNumber)),
                (Rule: isInvalid(intern.Status), Parameter: nameof(Intern.Status)),
                (Rule: isInvalid(intern.UpdatedDate), Parameter: nameof(Intern.UpdatedDate)),

                (Rule: IsNotSame(
                    firstDate: intern.UpdatedDate,
                    secondDate: intern.CreatedDate,
                    secondDateName: nameof(Intern.CreatedDate)),
                    Parameter: nameof(Intern.UpdatedDate)));
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

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not the same as {secondDateName}"
            };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
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
