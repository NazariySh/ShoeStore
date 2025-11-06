using ShoeStore.Infrastructure.Data.Initializers;

namespace ShoeStore.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> InitializeAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbInitializer = scope.ServiceProvider.GetRequiredService<IInitializer>();

        await dbInitializer.InitializeAsync();

        return app;
    }
}