
using FluentValidation;

namespace FreeBilling.Web.Validators
{
    public class ValidateEndpointFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            
            var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
            var model = context.Arguments.
            OfType<T>()
            .FirstOrDefault();
            if (model != null)
            {
                var validation = validator.Validate(model);
                if (!validation.IsValid)
                {
                    return Results.ValidationProblem(validation.ToDictionary());
                }
            }

            return await next(context);
            
        }
    }
}
