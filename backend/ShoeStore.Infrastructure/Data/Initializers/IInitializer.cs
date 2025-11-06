namespace ShoeStore.Infrastructure.Data.Initializers;

public interface IInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
}