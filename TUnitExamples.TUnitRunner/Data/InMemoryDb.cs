using TUnit.Core.Interfaces;

namespace TUnitExamples.TUnitRunner.Data;

public class InMemoryDb : IAsyncInitializer, IAsyncDisposable
{
    private Dictionary<string, string> _store = null!;

    public InMemoryDb()
    {
        Console.WriteLine("InMemoryDb constructor is executing");
    }

    public Task InitializeAsync()
    {
        // Simulate async setup - e.g. connecting to a database or starting a container
        Console.WriteLine("InMemoryDb InitializeAsync is executing");
        _store = new Dictionary<string, string>();
        return Task.CompletedTask;
    }

    public Task SetAsync(string key, string value)
    {
        _store[key] = value;
        return Task.CompletedTask;
    }

    public Task<string?> GetAsync(string key)
    {
        _store.TryGetValue(key, out var value);
        return Task.FromResult(value);
    }

    public ValueTask DisposeAsync()
    {
        // Simulate async teardown - e.g. closing connections, removing containers
        Console.WriteLine("InMemoryDb DisposeAsync is executing");
        _store.Clear();
        return ValueTask.CompletedTask;
    }
}
