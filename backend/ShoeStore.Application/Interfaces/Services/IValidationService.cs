namespace ShoeStore.Application.Interfaces.Services;

public interface IValidationService
{
    Task ValidateAsync<T>(T instance, CancellationToken cancellationToken = default);
}