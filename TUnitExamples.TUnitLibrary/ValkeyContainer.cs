using System.Diagnostics.CodeAnalysis;
using DotNet.Testcontainers.Containers;
using Testcontainers.Redis;
using TUnit.Core.Interfaces;

namespace TUnitExamples.TUnitLibrary;

public sealed class ValkeyContainer : IAsyncInitializer, IAsyncDisposable
{
    // Nested data source: this is initialized before InitializeAsync is called
    [ClassDataSource<DockerNetwork>(Shared = SharedType.PerTestSession)]
    public required DockerNetwork DockerNetwork { get; init; }

    public string ConnectionString => Instance.GetConnectionString();

    [field: AllowNull, MaybeNull]
    public IContainer Instance =>
        field ??= new RedisBuilder("valkey/valkey:9.0.3")
            .WithNetwork(DockerNetwork.Instance)
            .Build();

    public async Task InitializeAsync()
    {
        await Instance.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Instance.DisposeAsync();
    }
}
