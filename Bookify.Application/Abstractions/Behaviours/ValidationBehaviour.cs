using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Abstractions.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest: IBaseCommand
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) 
        {
            if (! _validators.Any())
            {
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);
            var validationErrors = _validators
                .Select(validator => validator.Validate(context))
                .Where(validationResult => validationResult.Errors.Any())
                .SelectMany(validationresult => validationresult.Errors)
                .Select(validationFailure => new ValidationError(
                      validationFailure.PropertyName,
                      validationFailure.ErrorMessage
                    ))
                .ToList();
            if (validationErrors.Any())
            {
                throw new Exceptions.ValidationException(validationErrors);
            }
            return await next();
        }
    }
}
