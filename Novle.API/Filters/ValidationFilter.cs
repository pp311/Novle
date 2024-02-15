using System.Reflection;
using FluentValidation;
using MediatR;

namespace Novel.API.Filters;

public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, 
        EndpointFilterDelegate next)
    {
        var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices).ToList();

        if (validationDescriptors.Any())
            return invocationContext => Validate(validationDescriptors, invocationContext, next);

        return next;
    }

    private static async ValueTask<object?> Validate(IEnumerable<ValidationDescriptor> validationDescriptors, 
        EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (ValidationDescriptor descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is not null)
            {
                var result = await descriptor.Validator.ValidateAsync(new ValidationContext<object>(argument));

                if (!result.IsValid)
                    return Results.ValidationProblem(result.ToDictionary());
            }
        }

        return await next.Invoke(invocationContext);
    }

    static IEnumerable<ValidationDescriptor> GetValidators(MethodInfo methodInfo, IServiceProvider serviceProvider)
    {
        ParameterInfo[] parameters = methodInfo.GetParameters();

        for (int i = 0; i < parameters.Length; i++)
        {
            ParameterInfo parameter = parameters[i];

            var isRequestParameter = parameter.ParameterType
                .GetInterfaces().ToList()
                .Exists(x => x.Name == nameof(IRequest));
            
            if (isRequestParameter)
            {
                Type validatorType = typeof(IValidator<>).MakeGenericType(parameter.ParameterType);

                if (serviceProvider.GetService(validatorType) is IValidator validator)
                {
                    yield return new ValidationDescriptor 
                    { 
                        ArgumentIndex = i, 
                        ArgumentType = parameter.ParameterType, 
                        Validator = validator 
                    };
                }
            }
        }
    }

    private class ValidationDescriptor
    {
        public required int ArgumentIndex { get; init; }
        public required Type ArgumentType { get; init; }
        public required IValidator Validator { get; init; }
    }
}