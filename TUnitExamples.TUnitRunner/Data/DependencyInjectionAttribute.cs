using Microsoft.Extensions.DependencyInjection;

namespace TUnitExamples.TUnitRunner.Data;

// Use this attribute when you don't want to use WebApplicationFactory to build your entire web
// application with all its services, and you only want to test a couple services with a few
// dependencies.
public class DependencyInjectionAttribute : DependencyInjectionDataSourceAttribute<IServiceScope>
{
    private static readonly IServiceProvider ServiceProvider = BuildProvider();

    public override IServiceScope CreateScope(DataGeneratorMetadata dataGeneratorMetadata)
    {
        return ServiceProvider.CreateScope();
    }

    public override object? Create(IServiceScope scope, Type type)
    {
        return scope.ServiceProvider.GetService(type);
    }

    private static IServiceProvider BuildProvider()
    {
        // Add services here
        return new ServiceCollection().BuildServiceProvider();
    }
}
