using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using TUnit.Core.Interfaces;

namespace TUnitExamples.TUnitLibrary;

public sealed class DockerNetwork : IAsyncInitializer, IAsyncDisposable
{
    public INetwork Instance { get; } = new NetworkBuilder().Build();

    public async Task InitializeAsync()
    {
        await Instance.CreateAsync();
    }

    public ValueTask DisposeAsync() => Instance.DisposeAsync();
}
