using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Games.RockPaperScissors.Domain;
using LanguageExt;
using LanguageExt.Common;

namespace Games.RockPaperScissors.Presentation.Validators
{
    public class RequestValidator : IRequestValidator
    {
        private readonly Dictionary<Type, IValidator> validators;

        public RequestValidator(IEnumerable<IValidator> validators)
        {
            this.validators = validators.ToDictionary(v => v.GetType().BaseType?.GenericTypeArguments[0]);
        }

        public Result<Unit> Validate<T>(T request)
        {
            return
                this.GetValidator<T>().Bind(validator =>
                this.Validate(validator, request));
        }

        private Result<IValidator<T>> GetValidator<T>()
        {
            return this.validators.ContainsKey(typeof(T))
                ? new Result<IValidator<T>>(this.validators[typeof(T)] as IValidator<T>)
                : new Result<IValidator<T>>(new Exception($"Validator of {nameof(T)} not found."));
        }

        private Result<Unit> Validate<T>(IValidator<T> validator, T request)
        {
            ValidationResult validationResult = validator.Validate(request);
            if (validationResult.IsValid)
            {
                return new Result<Unit>(Unit.Default);
            }
            else
            {
                return new Result<Unit>(
                    new ValidationException($"Validation of {request.ToString()} failed", validationResult.Errors));
            }
        }

    }
}