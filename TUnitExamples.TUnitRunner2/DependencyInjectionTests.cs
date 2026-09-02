using TUnitExamples.TUnitLibrary;

namespace TUnitExamples.TUnitRunner2;

[ClassDataSource<ValkeyContainer>(Shared = SharedType.PerTestSession)]
public class TestSessionSharedDependencyTest(ValkeyContainer valkey)
{
    [Test]
    public async Task ValkeyContainerIsCreatedOnce()
    {
        // Simulating a long-running test
        await Task.Delay(TimeSpan.FromSeconds(10));
        Console.WriteLine(valkey.ConnectionString);
    }
}
