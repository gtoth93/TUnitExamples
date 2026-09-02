using TUnitExamples.Common;
using TUnitExamples.TUnitLibrary;
using TUnitExamples.TUnitRunner.Data;

namespace TUnitExamples.TUnitRunner;

public class DependencyInjectionTests
{
    // A new InMemoryDb is created for each test
    [Test]
    [ClassDataSource<InMemoryDb>]
    public async Task Database_SetAndGet(InMemoryDb db)
    {
        await db.SetAsync("key", "value");

        var result = await db.GetAsync("key");

        await Assert.That(result).IsEqualTo("value");
    }

    // The same InMemoryDb instance is shared across all tests in this class
    [Test]
    [ClassDataSource<InMemoryDb>(Shared = SharedType.PerClass)]
    public async Task Database_SharedPerClass(InMemoryDb db)
    {
        await db.SetAsync("shared", "data");

        var result = await db.GetAsync("shared");

        await Assert.That(result).IsNotNull();
    }

    [Test]
    [ClassDataSource<InMemoryDb, InMemoryDb>]
    public async Task Database_SetAndGetTwoSources(InMemoryDb db1, InMemoryDb db2)
    {
        await db1.SetAsync("key1", "value1");
        await db2.SetAsync("key2", "value2");

        var db1Key1Value = await db1.GetAsync("key1");
        var db1Key2Value = await db1.GetAsync("key2");
        var db2Key1Value = await db2.GetAsync("key1");
        var db2Key2Value = await db2.GetAsync("key2");

        await Assert.That(db1Key1Value).IsEqualTo("value1");
        await Assert.That(db1Key2Value).IsNull();
        await Assert.That(db2Key1Value).IsNull();
        await Assert.That(db2Key2Value).IsEqualTo("value2");
    }

    [Test]
    [ClassDataSource<InMemoryDb, InMemoryDb>(Shared = [SharedType.PerClass, SharedType.PerClass])]
    public async Task Database_SetAndGetSharedSources(InMemoryDb db1, InMemoryDb db2)
    {
        await db1.SetAsync("key1", "value1");
        await db2.SetAsync("key2", "value2");

        var db1Key1Value = await db1.GetAsync("key1");
        var db1Key2Value = await db1.GetAsync("key2");
        var db2Key1Value = await db2.GetAsync("key1");
        var db2Key2Value = await db2.GetAsync("key2");

        await Assert.That(db1Key1Value).IsEqualTo("value1");
        await Assert.That(db1Key2Value).IsEqualTo("value2");
        await Assert.That(db2Key1Value).IsEqualTo("value1");
        await Assert.That(db2Key2Value).IsEqualTo("value2");
        await Assert.That(db1).IsSameReferenceAs(db2);
    }
}

// ClassDataSource can also be applied at the class level
[ClassDataSource<InMemoryDb>(Shared = SharedType.PerTestSession)]
public class SharedDatabaseTests(InMemoryDb db)
{
    // Or injected as a property instead of a constructor parameter
    [ClassDataSource<Calculator>]
    public required Calculator Calculator { get; init; }

    [Test]
    public async Task Calculator_ResultCanBeStored()
    {
        var result = Calculator.Add(2, 3);

        await db.SetAsync("result", result.ToString());

        await Assert.That(await db.GetAsync("result")).IsEqualTo("5");
    }

    // You can make a test depend on another test. This is only needed in systems where stateless tests
    // are not possible, extremely challenging or too slow.
    [Test]
    [DependsOn<SharedDatabaseTests>(nameof(Calculator_ResultCanBeStored))]
    public async Task Calculator_MoreResultCanBeStored()
    {
        var result = Calculator.Add(2, 3);

        await db.SetAsync("result", result.ToString());

        await Assert.That(await db.GetAsync("result")).IsEqualTo("5");
    }
}

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
