using FluentValidation;
using FluentValidation.Results;

namespace Defra.TradeImportsDataApi.Api.Endpoints;

public abstract class ValidationEndpointFilter<TArgument> : AbstractValidator<TArgument>, IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var requestParams = context.GetArgument<TArgument>(ArgumentIndex);
        var validationResult = await ValidateAsync(requestParams);

        if (validationResult.IsValid)
            return await next(context);

        return Results.ValidationProblem(GroupErrors(validationResult.Errors));
    }

    private static IEnumerable<KeyValuePair<string, string[]>> GroupErrors(List<ValidationFailure> failures) =>
        failures
            .GroupBy(e => e.PropertyName)
            .Select(e => new KeyValuePair<string, string[]>(e.Key, e.Select(error => error.ErrorMessage).ToArray()));

    // Assume the request's method signature has the request object as the first argument
    private static int ArgumentIndex => 0;
}
