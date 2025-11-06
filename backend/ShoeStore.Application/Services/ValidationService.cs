using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ShoeStore.Application.Interfaces.Services;

namespace ShoeStore.Application.Services;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task ValidateAsync<T>(T instance, CancellationToken cancellationToken = default)
    {
        var validator = GetValidator<T>();

        if (validator is null)
        {
            return;
        }

        var result = await validator.ValidateAsync(instance, cancellationToken);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }

    private IValidator<T>? GetValidator<T>()
    {
        return _serviceProvider.GetService<IValidator<T>>();
    }
}