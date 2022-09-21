using ErrorOr;
using FluentValidation;
using MediatR;

namespace Locoom.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if(_validator is null)
            {
                return await next();
            }

            // Get the validator from request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If no errors, then
            if(validationResult.IsValid)
            {
                // call the handler
                return await next();
            }

            // else, list it with ConvertAll which will make Select + ToList, to make a List of Error
            var errors = validationResult.Errors
                .ConvertAll(validationFailure => Error.Validation(
                                            validationFailure.PropertyName,
                                            validationFailure.ErrorMessage));

            return (dynamic)errors;
        }
    }
}
