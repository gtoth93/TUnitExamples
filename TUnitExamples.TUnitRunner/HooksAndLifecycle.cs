using System.Diagnostics.CodeAnalysis;
using TUnitExamples.TUnitRunner.Data;

[assembly: ExcludeFromCodeCoverage]

namespace TUnitExamples.TUnitRunner;

public static class GlobalHooks
{
    [Before(TestDiscovery)]
    public static Task BeforeTestDiscovery(BeforeTestDiscoveryContext context)
    {
        // you can change certain settings here like the number of maximum parallel tests
        context.Settings.Parallelism.MaximumParallelTests = 20;
        Console.WriteLine(
            "BeforeTestDiscovery: After this, test discovery will start, which includes scanning all assemblies for tests and creating data sources"
        );
        return Task.CompletedTask;
    }

    [After(TestDiscovery)]
    public static Task AfterTestDiscovery(TestDiscoveryContext context)
    {
        Console.WriteLine("AfterTestDiscovery: This runs right after test discovery");
        return Task.CompletedTask;
    }

    [Before(TestSession)]
    public static Task BeforeTestSession(TestSessionContext context)
    {
        Console.WriteLine(
            "BeforeTestSession: This runs before the first test in this session (executable)"
        );
        return Task.CompletedTask;
    }

    [BeforeEvery(Assembly)]
    public static Task BeforeEveryAssembly(AssemblyHookContext context)
    {
        Console.WriteLine("BeforeEveryAssembly: Runs before the first test in each assembly");
        return Task.CompletedTask;
    }

    [Before(Assembly)]
    public static Task BeforeAssembly(AssemblyHookContext context)
    {
        Console.WriteLine("BeforeAssembly: Runs before the first test in this assembly");
        return Task.CompletedTask;
    }

    [BeforeEvery(Class)]
    public static Task BeforeEveryClass(ClassHookContext context)
    {
        Console.WriteLine("BeforeEveryClass: Runs before the first test of each class");
        return Task.CompletedTask;
    }

    [BeforeEvery(Test)]
    public static Task BeforeEveryTest(TestContext context)
    {
        Console.WriteLine("BeforeEveryTest: Runs before each test");
        Console.WriteLine($"Isolated name: {context.Isolation.GetIsolatedName("isolated_name")}");
        Console.WriteLine($"Isolated prefix: {context.Isolation.GetIsolatedPrefix("__")}");
        return Task.CompletedTask;
    }

    [AfterEvery(Test)]
    public static Task AfterEveryTest(TestContext context)
    {
        Console.WriteLine("AfterEveryTest: Runs after each test");
        return Task.CompletedTask;
    }

    [AfterEvery(Class)]
    public static Task AfterEveryClass(ClassHookContext context)
    {
        Console.WriteLine("AfterEveryClass: Runs after the last test of each class");
        return Task.CompletedTask;
    }

    [After(Assembly)]
    public static Task AfterAssembly(AssemblyHookContext context)
    {
        Console.WriteLine("AfterAssembly: Runs after the last test in this assembly");
        return Task.CompletedTask;
    }

    [AfterEvery(Assembly)]
    public static Task AfterEveryAssembly(AssemblyHookContext context)
    {
        Console.WriteLine("AfterEveryAssembly: Runs after the last test in each assembly");
        return Task.CompletedTask;
    }

    [After(TestSession)]
    public static Task AfterTestSession(TestSessionContext context)
    {
        Console.WriteLine(
            "AfterTestSession: Runs after the last test in this session (executable)"
        );
        return Task.CompletedTask;
    }
}

public class HooksAndLifecycleTests
{
    public HooksAndLifecycleTests()
    {
        Console.WriteLine("HooksAndLifecycleTests constructor is executing");
    }

    [ClassDataSource<InMemoryDb>(Shared = SharedType.PerTestSession)]
    public required InMemoryDb InMemoryDb
    {
        get;
        init
        {
            Console.WriteLine("InMemoryDb is set on the test class property");
            field = value;
        }
    }

    [Before(Class)]
    public static Task BeforeClass(ClassHookContext context)
    {
        Console.WriteLine("BeforeClass: Runs before the first test of this class");
        return Task.CompletedTask;
    }

    [Before(Test)]
    public Task BeforeTest(TestContext context)
    {
        Console.WriteLine("BeforeTest: Runs before each test in this class");
        return Task.CompletedTask;
    }

    [Test]
    public void LifecycleTest()
    {
        Console.WriteLine("Test is executing");
    }

    [After(Test)]
    public Task AfterTest(TestContext context)
    {
        Console.WriteLine("AfterTest: Runs after each test in this class");
        context.Output.WriteLine($"{context.Metadata.DisplayName} ran successfully");

        // if (context.Execution.Result?.State == TestState.Failed)
        // {
        //     context.Output.WriteError($"{context.Metadata.DisplayName} has failed");
        //     context.Output.AttachArtifact(
        //         new Artifact
        //         {
        //             File = new FileInfo("path/to/logfile_or_screenshot"),
        //             DisplayName = "App screenshot",
        //             Description = "Screenshot of the app",
        //         }
        //     );
        // }
        return Task.CompletedTask;
    }

    [After(Class)]
    public static Task AfterClass(ClassHookContext context)
    {
        Console.WriteLine("AfterClass: Runs after the last test of this class");
        return Task.CompletedTask;
    }
}
