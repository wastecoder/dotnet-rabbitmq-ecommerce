using FluentValidation;

namespace SalesService.Api.Application.Helpers;

public static class ValidatorExtensions
{
    public static async Task ValidateOrThrowAsync<T>(this IValidator<T> validator, T instance)
    {
        var result = await validator.ValidateAsync(instance);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}